using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class SaleReturnRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<SaleReturn>(context, currentTenant), ISaleReturnRepository
{
}