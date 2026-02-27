using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Models.Clinic;

namespace ErpManagement.DataAccess.Repositories.Shared;

public class AppointmentItemRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<AppointmentItem>(context, currentTenant), IAppointmentItemRepository
{
}
