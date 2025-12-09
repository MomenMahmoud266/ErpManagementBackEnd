using ErpManagement.Domain.DTOs.Request.People.Supplier;
using ErpManagement.Domain.DTOs.Response.People.Supplier;

namespace ErpManagement.Services.IServices.People;

public interface ISupplierService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<SupplierGetAllResponse>> GetAllAsync(RequestLangEnum lang, SupplierGetAllFiltrationsRequest model);
    Task<Response<SupplierCreateRequest>> CreateAsync(SupplierCreateRequest model);
    Task<Response<SupplierGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<SupplierUpdateRequest>> UpdateAsync(int id, SupplierUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}