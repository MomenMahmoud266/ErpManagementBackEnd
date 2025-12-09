using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Organization;
using ErpManagement.Domain.Models.Organization;

namespace ErpManagement.DataAccess.Repositories.Organization;

public class WarehouseRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<Warehouse>(context, currentTenant), IWarehouseRepository
{
}