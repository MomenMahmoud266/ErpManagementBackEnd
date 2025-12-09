using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Organization;
using ErpManagement.Domain.Models.Organization;

namespace ErpManagement.DataAccess.Repositories.Organization;

public class CompanyRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<Company>(context, currentTenant), ICompanyRepository
{
}