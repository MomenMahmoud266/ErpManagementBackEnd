using ErpManagement.Domain.DTOs.Request.Shared;
using ErpManagement.Domain.DTOs.Response.Shared;

namespace ErpManagement.Services.IServices.Products;

public interface IProductTypeService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<ProductTypeGetAllResponse>> GetAllAsync(RequestLangEnum lang, ProductTypeGetAllFiltrationsRequest model);
    Task<Response<ProductTypeGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<ProductTypeCreateRequest>> CreateAsync(ProductTypeCreateRequest model);
    Task<Response<ProductTypeUpdateRequest>> UpdateAsync(int id, ProductTypeUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}