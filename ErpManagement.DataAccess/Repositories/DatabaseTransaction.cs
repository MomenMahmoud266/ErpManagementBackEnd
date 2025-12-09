using Microsoft.EntityFrameworkCore.Storage;
using ErpManagement.Domain.Interfaces;

namespace ErpManagement.DataAccess.Repositories
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public DatabaseTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}