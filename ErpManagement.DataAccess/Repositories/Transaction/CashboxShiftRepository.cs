using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Transaction;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transaction;

public class CashboxShiftRepository : BaseRepository<CashboxShift>, ICashboxShiftRepository
{
    public CashboxShiftRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
        : base(context, currentTenant)
    {
    }
}
