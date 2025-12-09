using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transaction;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transaction;

public class StockAdjustmentItemRepository : BaseRepository<StockAdjustmentItem>, IStockAdjustmentItemRepository
{
    public StockAdjustmentItemRepository(ErpManagementDbContext context, ICurrentTenant currentTenant) : base(context, currentTenant)
    {
    }
}