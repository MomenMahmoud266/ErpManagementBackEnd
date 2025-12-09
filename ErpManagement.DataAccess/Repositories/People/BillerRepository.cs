using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.People;
using ErpManagement.Domain.Models.People;

namespace ErpManagement.DataAccess.Repositories.People;

public class BillerRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
    : BaseRepository<Biller>(context, currentTenant), IBillerRepository
{
}