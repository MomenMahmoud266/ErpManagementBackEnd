using ErpManagement.DataAccess.DbContext;

namespace ErpManagement.DataAccess.Repositories.Auth;


public class UserRoleRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<ApplicationUserRole>(context, currentTenant), IUserRoleRepository
{
}