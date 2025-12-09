using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.DataAccess.Repositories.Shared;

public class BrandRepository(ErpManagementDbContext context, ICurrentTenant currentTenant) 
    : BaseRepository<Brand>(context, currentTenant), IBrandRepository
{
}