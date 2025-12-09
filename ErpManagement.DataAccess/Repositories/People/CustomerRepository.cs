using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.People;
using ErpManagement.Domain.Models.People;

namespace ErpManagement.DataAccess.Repositories.People;

public class CustomerRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<Customer>(context, currentTenant), ICustomerRepository
{
}
