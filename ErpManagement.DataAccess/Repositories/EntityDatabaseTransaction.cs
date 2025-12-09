using ErpManagement.DataAccess.DbContext;

namespace ErpManagement.DataAccess.Repositories;

public class EntityDatabaseTransaction(ErpManagementDbContext context) : IDatabaseTransaction
{
    private readonly IDbContextTransaction _transaction = context.Database.BeginTransactionAsync().Result;

    public async Task CommitAsync() =>
        await _transaction.CommitAsync();

    public async Task RollbackAsync() =>
        await _transaction.RollbackAsync();

    public async Task DisposeAsync() =>
        await _transaction.DisposeAsync();

    public void Dispose() =>
        _transaction.Dispose();
}
