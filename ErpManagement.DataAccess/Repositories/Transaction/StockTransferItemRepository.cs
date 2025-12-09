using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transaction;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transaction;

public class StockTransferItemRepository : BaseRepository<StockTransferItem>, IStockTransferItemRepository
{
    public StockTransferItemRepository(ErpManagementDbContext context, ICurrentTenant currentTenant) : base(context, currentTenant)
    {
    }
}