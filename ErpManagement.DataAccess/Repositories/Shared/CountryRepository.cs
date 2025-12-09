using ErpManagement.DataAccess.DbContext;
using ErpManagement.DataAccess.Repositories;
using ErpManagement.Domain.Interfaces.Repositories.Shared;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.DataAccess.Repositories.Shared;

public class CountryRepository(ErpManagementDbContext context, ICurrentTenant currentTenant) 
    : BaseRepository<Country>(context, currentTenant), ICountryRepository
{
}