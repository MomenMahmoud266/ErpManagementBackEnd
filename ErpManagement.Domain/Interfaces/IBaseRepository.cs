public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<TType>> GetSpecificSelectAsync<TType>(
        Expression<Func<T, bool>>? filter,
        Expression<Func<T, TType>> select,
        string? includeProperties = null,
        bool ignoreQueryFilters = false,
        int pageNumber = 0,
        int pageSize = 0,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where TType : class;

    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        bool ignoreQueryFilters = false,
        int? skip = null,
        int? take = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

    Task<T> CreateAsync(T entity);

    Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities);

    Task<T?> GetByIdAsync(int id);

    Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter,
            string? includeProperties = null);

    T Update(T entity);

    void UpdateRange(IEnumerable<T> entities);

    T Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    Task<bool> ExistAsync(int id);

    Task<bool> ExistAsync(
        Expression<Func<T, bool>> filter,
        string? includeProperties = null);

    Task<int> CountAsync(
        Expression<Func<T, bool>>? filter,
        string? includeProperties = null);

    Task<int> CountAsync();

    Task<bool> AnyAsync(Expression<Func<T, bool>>? filter);

    Task<bool> ExecuteDeleteAsync(Expression<Func<T, bool>> filter);
}
