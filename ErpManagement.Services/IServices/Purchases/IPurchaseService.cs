using ErpManagement.Domain.DTOs.Request.Purchases;
using ErpManagement.Domain.DTOs.Response.Purchases;

namespace ErpManagement.Services.IServices.Transactions;

public interface IPurchaseService
{
    Task<Response<PurchaseGetAllResponse>> GetAllAsync(RequestLangEnum lang, PurchaseGetAllFiltrationsForPurchasesRequest model);
    Task<Response<PurchaseGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<PurchaseCreateRequest>> CreateAsync(PurchaseCreateRequest model);
    Task<Response<PurchaseUpdateRequest>> UpdateAsync(int id, PurchaseUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}