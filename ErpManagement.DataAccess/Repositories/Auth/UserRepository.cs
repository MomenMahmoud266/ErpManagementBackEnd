using ErpManagement.DataAccess.DbContext;

namespace ErpManagement.DataAccess.Repositories.Auth;

public class UserRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<ApplicationUser>(context, currentTenant), IUserRepository
{
}