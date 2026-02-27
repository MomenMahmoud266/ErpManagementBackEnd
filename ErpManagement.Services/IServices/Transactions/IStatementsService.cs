using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;

namespace ErpManagement.Services.IServices.Transactions;

public interface IStatementsService
{
    Task<Response<CustomerStatementResponse>> GetCustomerStatementAsync(RequestLangEnum lang, CustomerStatementRequest model);
    Task<Response<SupplierStatementResponse>> GetSupplierStatementAsync(RequestLangEnum lang, SupplierStatementRequest model);
}
