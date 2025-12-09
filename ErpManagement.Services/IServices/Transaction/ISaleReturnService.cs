using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface ISaleReturnService
{
    Task<Response<SaleReturnGetAllResponse>> GetAllAsync(RequestLangEnum lang, SaleReturnGetAllFiltrationsForSaleReturnsRequest model);
    Task<Response<SaleReturnGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<SaleReturnCreateRequest>> CreateAsync(SaleReturnCreateRequest model);
    Task<Response<SaleReturnUpdateRequest>> UpdateAsync(int id, SaleReturnUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}