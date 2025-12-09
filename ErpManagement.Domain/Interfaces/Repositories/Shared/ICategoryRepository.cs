using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Interfaces.Repositories.Shared;

public interface ICategoryRepository : IBaseRepository<Category>
{
    // Add custom methods if needed, otherwise inherit from IBaseRepository
}