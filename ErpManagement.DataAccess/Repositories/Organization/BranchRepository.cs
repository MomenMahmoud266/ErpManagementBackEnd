using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Organization;
using ErpManagement.Domain.Models.Organization;

namespace ErpManagement.DataAccess.Repositories.Organization;

public class BranchRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<Branch>(context, currentTenant), IBranchRepository
{
}