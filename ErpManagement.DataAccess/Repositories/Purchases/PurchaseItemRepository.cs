using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class PurchaseItemRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<PurchaseItem>(context, currentTenant), IPurchaseItemRepository
{

    public async Task<IEnumerable<PurchaseItem>> GetByPurchaseIdAsync(int purchaseId)
    {
        return await GetAllAsync(x => x.PurchaseId == purchaseId, includeProperties: "Product");
    }
}