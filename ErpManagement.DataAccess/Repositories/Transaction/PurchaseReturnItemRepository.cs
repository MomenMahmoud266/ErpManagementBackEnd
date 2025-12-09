// ErpManagement.DataAccess\Repositories\Transactions\PurchaseReturnItemRepository.cs
using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class PurchaseReturnItemRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<PurchaseReturnItem>(context, currentTenant), IPurchaseReturnItemRepository
{
}