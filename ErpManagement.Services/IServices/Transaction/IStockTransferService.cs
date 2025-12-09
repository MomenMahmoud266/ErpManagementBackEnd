using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Response.Transaction;

namespace ErpManagement.Services.IServices.Transactions;

public interface IStockTransferService
{
    Task<Response<StockTransferGetAllResponse>> GetAllAsync(RequestLangEnum lang, StockTransferGetAllFiltrationsForStockTransfersRequest model);
    Task<Response<StockTransferGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<StockTransferCreateRequest>> CreateAsync(StockTransferCreateRequest model);
    Task<Response<StockTransferUpdateRequest>> UpdateAsync(int id, StockTransferUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}