using ErpManagement.DataAccess.DbContext;

namespace ErpManagement.DataAccess.Repositories.StaticDataRepository;

public class StaticDataRepository(ErpManagementDbContext context) : IStaticDataRepository
{
    private readonly ErpManagementDbContext _context = context;

    public async Task<IEnumerable<SelectListResponse>> GetAllGenders(RequestLangEnum lang) =>
        await _context.Genders.AsNoTracking().Select(x => new SelectListResponse()
        {
            Id = x.Id,
            Name = lang == RequestLangEnum.Ar ? x.NameAr : lang == RequestLangEnum.En ? x.NameEn : x.NameTr
        }).ToListAsync();
}
