using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface IExpenseCategoryService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang);
    Task<Response<ExpenseCategoryGetAllResponse>> GetAllAsync(RequestLangEnum lang, ExpenseCategoryGetAllFiltrationsForExpenseCategoriesRequest model);
    Task<Response<ExpenseCategoryCreateRequest>> CreateAsync(ExpenseCategoryCreateRequest model);
    Task<Response<ExpenseCategoryGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<ExpenseCategoryUpdateRequest>> UpdateAsync(int id, ExpenseCategoryUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}