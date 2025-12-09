using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class SaleItemRepository : BaseRepository<SaleItem>, ISaleItemRepository
{
    public SaleItemRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
        : base(context, currentTenant)
    {
    }
}