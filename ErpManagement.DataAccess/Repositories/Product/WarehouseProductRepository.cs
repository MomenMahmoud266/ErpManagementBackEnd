using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Products;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.DataAccess.Repositories.Products;

public class WarehouseProductRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<WarehouseProduct>(context, currentTenant), IWarehouseProductRepository
{
}