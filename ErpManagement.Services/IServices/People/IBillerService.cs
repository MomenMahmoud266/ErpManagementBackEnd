using ErpManagement.Domain.DTOs.Request.People.Biller;
using ErpManagement.Domain.DTOs.Response.People.Biller;

namespace ErpManagement.Services.IServices.People;

public interface IBillerService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<BillerGetAllResponse>> GetAllAsync(RequestLangEnum lang, BillerGetAllFiltrationsRequest model);
    Task<Response<BillerCreateRequest>> CreateAsync(BillerCreateRequest model);
    Task<Response<BillerGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<BillerUpdateRequest>> UpdateAsync(int id, BillerUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}