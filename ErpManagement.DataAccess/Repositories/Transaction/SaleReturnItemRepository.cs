using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class SaleReturnItemRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<SaleReturnItem>(context, currentTenant), ISaleReturnItemRepository
{
}