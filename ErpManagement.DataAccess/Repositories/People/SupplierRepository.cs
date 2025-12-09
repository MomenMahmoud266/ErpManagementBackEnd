using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.People;
using ErpManagement.Domain.Models.People;

namespace ErpManagement.DataAccess.Repositories.People;

public class SupplierRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<Supplier>(context, currentTenant), ISupplierRepository
{
}