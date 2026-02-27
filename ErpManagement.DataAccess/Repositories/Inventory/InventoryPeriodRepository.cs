using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Inventory;
using ErpManagement.Domain.Models.Inventory;

namespace ErpManagement.DataAccess.Repositories.Inventory;

public class InventoryPeriodRepository : BaseRepository<InventoryPeriod>, IInventoryPeriodRepository
{
    public InventoryPeriodRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
        : base(context, currentTenant)
    {
    }
}
