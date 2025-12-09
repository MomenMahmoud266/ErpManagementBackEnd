using ErpManagement.Domain.DTOs.Request.Organization.Company;
using ErpManagement.Domain.DTOs.Response.Organization.Company;

namespace ErpManagement.Services.IServices.Organization;

public interface ICompanyService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<CompanyGetAllResponse>> GetAllAsync(RequestLangEnum lang, CompanyGetAllFiltrationsRequest model);
    Task<Response<CompanyCreateRequest>> CreateAsync(CompanyCreateRequest model);
    Task<Response<CompanyGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<CompanyUpdateRequest>> UpdateAsync(int id, CompanyUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}