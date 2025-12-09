using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Products;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.DataAccess.Repositories.Shared;

public class VariantRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<Variant>(context, currentTenant), IVariantRepository
{
}