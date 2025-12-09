using ErpManagement.Domain.DTOs.Request.Products;
using ErpManagement.Domain.DTOs.Response.Products;

namespace ErpManagement.Services.IServices.Products;

public interface IProductService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<ProductGetAllResponse>> GetAllAsync(RequestLangEnum lang, ProductGetAllFiltrationsRequest model);
    Task<Response<ProductGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<ProductCreateRequest>> CreateAsync(ProductCreateRequest model);
    Task<Response<ProductUpdateRequest>> UpdateAsync(int id, ProductUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}