using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Interfaces.Repositories.Transactions;

public interface IStockMovementRepository : IBaseRepository<StockMovement>
{
    // additional purchase-specific queries can be added later
}