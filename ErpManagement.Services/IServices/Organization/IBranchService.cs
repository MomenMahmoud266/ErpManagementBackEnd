using ErpManagement.Domain.DTOs.Request.Organization.Branch;
using ErpManagement.Domain.DTOs.Response.Organization.Branch;

namespace ErpManagement.Services.IServices.Organization;

public interface IBranchService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<BranchGetAllResponse>> GetAllAsync(RequestLangEnum lang, BranchGetAllFiltrationsRequest model);
    Task<Response<BranchCreateRequest>> CreateAsync(BranchCreateRequest model);
    Task<Response<BranchGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<BranchUpdateRequest>> UpdateAsync(int id, BranchUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}