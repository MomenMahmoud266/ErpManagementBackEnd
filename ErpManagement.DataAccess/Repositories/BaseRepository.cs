namespace ErpManagement.DataAccess.Repositories;

using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Models.Shared;

public class BaseRepository<T>(ErpManagementDbContext context, ICurrentTenant currentTenant) : IBaseRepository<T> where T : class
{
    protected ErpManagementDbContext _context = context;
    protected readonly ICurrentTenant _currentTenant = currentTenant;
    internal DbSet<T> dbSet = context.Set<T>();
    private static readonly char[] separator = [','];

    public async Task<IEnumerable<TType>> GetSpecificSelectAsync<TType>(
        Expression<Func<T, bool>>? filter,
        Expression<Func<T, TType>> select,
        string? includeProperties = null,
        bool ignoreQueryFilters = false,
        int pageNumber = 0,
        int pageSize = 0,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where TType : class
    {
        IQueryable<T> query = includeProperties is null ? dbSet.AsNoTracking() : dbSet;

        query = includeProperties is not null ? GetIncludedData(includeProperties, separator, query) : query;

        if (filter is not null)
            query = query.Where(filter);

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (pageSize > 0)
        {
            var paginationResult = ImplementPagination(pageSize, pageNumber);

                query = query.Skip(paginationResult.Skip);
                query = query.Take(paginationResult.Take);
        }

        if (orderBy is not null)
            query = orderBy(query);

        return await query.Select(select).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        bool ignoreQueryFilters = false,
        int? skip = null,
        int? take = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        IQueryable<T> query = includeProperties is null ? dbSet.AsNoTracking() : dbSet;

        query = includeProperties is not null ? GetIncludedData(includeProperties, separator, query) : query;

        if (filter is not null)
            query = query.Where(filter);

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        if (orderBy is not null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        // Auto-assign TenantId for tenant-scoped entities when not set
        if (entity is ITenantEntity tenantEntity && tenantEntity.TenantId == 0)
        {
            tenantEntity.TenantId = _currentTenant.TenantId;
        }

        await dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities)
    {
        // Auto-assign TenantId for tenant-scoped entities when not set
        foreach (var entity in entities)
        {
            if (entity is ITenantEntity tenantEntity && tenantEntity.TenantId == 0)
            {
                tenantEntity.TenantId = _currentTenant.TenantId;
            }
        }

        await dbSet.AddRangeAsync(entities);
        return entities;
    }

    public async Task<T?> GetByIdAsync(int id) =>
   await dbSet.FindAsync(id);

    public async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null)
    {
        IQueryable<T> query = includeProperties is null ? dbSet.AsNoTracking() : dbSet;

        query = includeProperties is not null ? GetIncludedData(includeProperties, separator, query) : query;

        if (filter is not null)
            query = query.Where(filter);

        return await query.FirstOrDefaultAsync();
    }

    public T Update(T entity)
    {
        dbSet.Update(entity);
        return entity;
    }

    public void UpdateRange(IEnumerable<T> entities) =>
        dbSet.UpdateRange(entities);

    public T Delete(T entity)
    {
        dbSet.Remove(entity);
        return entity;
    }

    public void DeleteRange(IEnumerable<T> entities) =>
        dbSet.RemoveRange(entities);

    public async Task<bool> ExistAsync(int id) =>
     await dbSet.FindAsync(id) is not null;

    public async Task<bool> ExistAsync(
        Expression<Func<T, bool>> filter,
        string? includeProperties = null)
    {
        IQueryable<T> query = includeProperties is null ? dbSet.AsNoTracking() : dbSet;

        query = includeProperties is not null ? GetIncludedData(includeProperties, separator, query) : query;

        if (filter is not null)
            query = query.Where(filter);

        return await query.FirstOrDefaultAsync() is not null;
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = includeProperties is null ? dbSet.AsNoTracking() : dbSet;

        query = includeProperties is not null ? GetIncludedData(includeProperties, separator, query) : query;

        if (filter is not null)
            query = query.Where(filter);

        return await query.CountAsync();
    }

    public async Task<int> CountAsync()
    {
        IQueryable<T> query = dbSet.AsNoTracking();

        return await query.CountAsync();
    }


    public async Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = dbSet.AsNoTracking();

        if (filter is not null)
            query = query.Where(filter);

        return await query.AnyAsync();
    }

    public async Task<bool> ExecuteDeleteAsync(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = dbSet.AsNoTracking();

        return  await query
            .Where(filter)
            .ExecuteDeleteAsync() > 0;
    }

    private IQueryable<T> GetIncludedData(string? includeProperties, char[] separator, IQueryable<T> query)
    {
        includeProperties?
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .ToList()
            .ForEach(includeProperty => query = query.Include(includeProperty).AsSplitQuery().AsNoTracking());

        return query;
    }


    private static GetPaginationResult ImplementPagination(int pageSize = 10, int pageNumber = 1) =>
        new()
        {
            Take = pageSize,
            Skip = (pageNumber - 1) * pageSize
        };
}
