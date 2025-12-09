using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transaction;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transaction;

public class StockAdjustmentRepository : BaseRepository<StockAdjustment>, IStockAdjustmentRepository
{
    public StockAdjustmentRepository(ErpManagementDbContext context, ICurrentTenant currentTenant) : base(context, currentTenant)
    {
    }
}