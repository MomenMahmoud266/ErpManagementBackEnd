using ErpManagement.Domain.DTOs.Request.Organization.Warehouse;
using ErpManagement.Domain.DTOs.Response.Organization.Warehouse;

namespace ErpManagement.Services.IServices.Organization;

public interface IWarehouseService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<WarehouseGetAllResponse>> GetAllAsync(RequestLangEnum lang, WarehouseGetAllFiltrationsRequest model);
    Task<Response<WarehouseCreateRequest>> CreateAsync(WarehouseCreateRequest model);
    Task<Response<WarehouseGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<WarehouseUpdateRequest>> UpdateAsync(int id, WarehouseUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}