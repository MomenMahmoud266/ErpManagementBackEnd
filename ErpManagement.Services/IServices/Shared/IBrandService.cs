namespace ErpManagement.Services.IServices.Shared;

public interface IBrandService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<BrandGetAllResponse>> GetAllAsync(RequestLangEnum lang, BrandGetAllFiltrationsRequest model);
    Task<Response<BrandGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<BrandCreateRequest>> CreateAsync(BrandCreateRequest model);
    Task<Response<BrandUpdateRequest>> UpdateAsync(int id, BrandUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}