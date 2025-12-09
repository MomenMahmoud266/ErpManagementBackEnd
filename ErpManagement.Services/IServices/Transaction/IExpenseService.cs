using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface IExpenseService
{
    Task<Response<ExpenseGetAllResponse>> GetAllAsync(RequestLangEnum lang, ExpenseGetAllFiltrationsForExpensesRequest model);
    Task<Response<ExpenseGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<ExpenseCreateRequest>> CreateAsync(ExpenseCreateRequest model);
    Task<Response<ExpenseUpdateRequest>> UpdateAsync(int id, ExpenseUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}