using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.DataAccess.Repositories.Shared;

public class CategoryRepository(ErpManagementDbContext context, ICurrentTenant currentTenant) 
    : BaseRepository<Category>(context, currentTenant), ICategoryRepository
{
}