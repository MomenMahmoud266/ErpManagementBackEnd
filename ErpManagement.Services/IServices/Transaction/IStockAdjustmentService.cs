using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Response.Transaction;

namespace ErpManagement.Services.IServices.Transactions;

public interface IStockAdjustmentService
{
    Task<Response<StockAdjustmentGetAllResponse>> GetAllAsync(RequestLangEnum lang, StockAdjustmentGetAllFiltrationsForStockAdjustmentsRequest model);
    Task<Response<StockAdjustmentGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<StockAdjustmentCreateRequest>> CreateAsync(StockAdjustmentCreateRequest model);
    Task<Response<StockAdjustmentUpdateRequest>> UpdateAsync(int id, StockAdjustmentUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}