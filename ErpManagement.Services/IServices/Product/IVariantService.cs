using ErpManagement.Domain.DTOs.Request.Shared.Variant;
using ErpManagement.Domain.DTOs.Response.Shared.Variant;

namespace ErpManagement.Services.IServices.Shared;

public interface IVariantService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<VariantGetAllResponse>> GetAllAsync(RequestLangEnum lang, VariantGetAllFiltrationsRequest model);
    Task<Response<VariantGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<VariantCreateRequest>> CreateAsync(VariantCreateRequest model);
    Task<Response<VariantUpdateRequest>> UpdateAsync(int id, VariantUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}