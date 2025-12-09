using ErpManagement.Domain.DTOs.Request.Shared.Tax;
using ErpManagement.Domain.DTOs.Response.Shared.Tax;

namespace ErpManagement.Services.IServices.Shared;

public interface ITaxService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<TaxGetAllResponse>> GetAllAsync(RequestLangEnum lang, TaxGetAllFiltrationsRequest model);
    Task<Response<TaxGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<TaxCreateRequest>> CreateAsync(TaxCreateRequest model);
    Task<Response<TaxUpdateRequest>> UpdateAsync(int id, TaxUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}