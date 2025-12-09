using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Products;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.DataAccess.Repositories.Products;

public class ProductTypeRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<ProductType>(context, currentTenant), IProductTypeRepository
{
}