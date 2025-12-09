// ErpManagement.DataAccess\Repositories\Transactions\PurchaseReturnRepository.cs
using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class PurchaseReturnRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<PurchaseReturn>(context, currentTenant), IPurchaseReturnRepository
{
}