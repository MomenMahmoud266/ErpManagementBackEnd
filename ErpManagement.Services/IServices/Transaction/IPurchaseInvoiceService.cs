// ErpManagement.Services.IServices.Transactions\IPurchaseInvoiceService.cs
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface IPurchaseInvoiceService
{
    Task<Response<PurchaseInvoiceGetAllResponse>> GetAllAsync(RequestLangEnum lang, PurchaseInvoiceGetAllFiltrationsForPurchaseInvoicesRequest model);
    Task<Response<PurchaseInvoiceGetByIdResponse>> GetByIdAsync(int id);
}