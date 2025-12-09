namespace ErpManagement.Domain.Interfaces.StaticDataRepository;

public interface IStaticDataRepository 
{
    Task<IEnumerable<SelectListResponse>> GetAllGenders(RequestLangEnum lang);
}
