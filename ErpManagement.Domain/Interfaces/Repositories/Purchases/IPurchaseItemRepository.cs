using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Interfaces.Repositories.Transactions;

public interface IPurchaseItemRepository : IBaseRepository<PurchaseItem>
{
    Task<IEnumerable<PurchaseItem>> GetByPurchaseIdAsync(int purchaseId);
}