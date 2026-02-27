using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface ISaleService
{
    Task<Response<SaleGetAllResponse>> GetAllAsync(RequestLangEnum lang, SaleGetAllFiltrationsForSalesRequest model);
    Task<Response<SaleGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<SaleGetByIdResponse>> CreateAsync(SaleCreateRequest model);
    Task<Response<SaleUpdateRequest>> UpdateAsync(int id, SaleUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}