using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Transaction;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transaction;

public class CashboxRepository : BaseRepository<Cashbox>, ICashboxRepository
{
    public CashboxRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
        : base(context, currentTenant)
    {
    }
}
