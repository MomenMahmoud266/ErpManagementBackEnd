using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Models.Gym;

namespace ErpManagement.DataAccess.Repositories.Shared;

public class MembershipPlanRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<MembershipPlan>(context, currentTenant), IMembershipPlanRepository
{
}
