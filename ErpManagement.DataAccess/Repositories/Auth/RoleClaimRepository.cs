using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces.Repositories.People;

namespace ErpManagement.DataAccess.Repositories.Auth;
public class RoleClaimRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<ApplicationRoleClaim>(context, currentTenant), IRoleClaimRepository
{
}