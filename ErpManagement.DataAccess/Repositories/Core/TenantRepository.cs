using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Core;
using ErpManagement.Domain.Models.Core;

namespace ErpManagement.DataAccess.Repositories.Core;

public class TenantRepository : BaseRepository<Tenant>, ITenantRepository
{
    public TenantRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
        : base(context, currentTenant)
    {
    }
}
