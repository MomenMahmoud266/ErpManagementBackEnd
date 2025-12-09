using ErpManagement.DataAccess.DbContext;

namespace ErpManagement.DataAccess.Repositories.Auth;

public class UserDeviceRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<ApplicationUserDevice>(context, currentTenant), IUserDeviceRepository
{
}