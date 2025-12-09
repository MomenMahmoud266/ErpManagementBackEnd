using ErpManagement.Domain.DTOs.Request.People.Customer;
using ErpManagement.Domain.DTOs.Response.People.Customer;

namespace ErpManagement.Services.IServices.Shared;

public interface ICustomerService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<CustomerGetAllResponse>> GetAllAsync(RequestLangEnum lang, CustomerGetAllFiltrationsRequest model);
    Task<Response<CustomerCreateRequest>> CreateAsync(CustomerCreateRequest model);
    Task<Response<CustomerGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<CustomerUpdateRequest>> UpdateAsync(int id, CustomerUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}