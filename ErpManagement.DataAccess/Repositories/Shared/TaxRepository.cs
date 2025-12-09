using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.DataAccess.Repositories.Shared;

public class TaxRepository(ErpManagementDbContext context, ICurrentTenant currentTenant) 
    : BaseRepository<Tax>(context, currentTenant), ITaxRepository
{
}