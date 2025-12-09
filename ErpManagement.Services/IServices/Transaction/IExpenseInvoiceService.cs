// ErpManagement.Services.IServices.Transactions\IExpenseInvoiceService.cs
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface IExpenseInvoiceService
{
    Task<Response<ExpenseInvoiceGetAllResponse>> GetAllAsync(RequestLangEnum lang, ExpenseInvoiceGetAllFiltrationsForExpenseInvoicesRequest model);
    Task<Response<ExpenseInvoiceGetByIdResponse>> GetByIdAsync(int id);
}