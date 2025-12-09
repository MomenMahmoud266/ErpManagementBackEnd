// ErpManagement.Services\IServices\Transactions\IPurchaseReturnService.cs
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface IPurchaseReturnService
{
    Task<Response<PurchaseReturnGetAllResponse>> GetAllAsync(RequestLangEnum lang, PurchaseReturnGetAllFiltrationsForPurchaseReturnsRequest model);
    Task<Response<PurchaseReturnGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<PurchaseReturnCreateRequest>> CreateAsync(PurchaseReturnCreateRequest model);
    Task<Response<PurchaseReturnUpdateRequest>> UpdateAsync(int id, PurchaseReturnUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}