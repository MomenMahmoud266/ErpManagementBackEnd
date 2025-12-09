// ErpManagement.Services.IServices.Transactions\ISaleInvoiceService.cs
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface ISaleInvoiceService
{
    Task<Response<SaleInvoiceGetAllResponse>> GetAllAsync(RequestLangEnum lang, SaleInvoiceGetAllFiltrationsForSaleInvoicesRequest model);
    Task<Response<SaleInvoiceGetByIdResponse>> GetByIdAsync(int id);
}