using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class StockMovementRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<StockMovement>(context, currentTenant), IStockMovementRepository
{
}