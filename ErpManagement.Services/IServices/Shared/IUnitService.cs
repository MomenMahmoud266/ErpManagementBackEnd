namespace ErpManagement.Services.IServices.Shared;

public interface IUnitService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<UnitGetAllResponse>> GetAllAsync(RequestLangEnum lang, UnitGetAllFiltrationsRequest model);
    Task<Response<UnitGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<UnitCreateRequest>> CreateAsync(UnitCreateRequest model);
    Task<Response<UnitUpdateRequest>> UpdateAsync(int id, UnitUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}