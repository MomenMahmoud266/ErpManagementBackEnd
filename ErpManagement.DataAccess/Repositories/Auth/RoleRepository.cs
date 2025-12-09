using ErpManagement.DataAccess.DbContext;

namespace ErpManagement.DataAccess.Repositories.Auth;


public class RoleRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<ApplicationRole>(context, currentTenant), IRoleRepository
{
}