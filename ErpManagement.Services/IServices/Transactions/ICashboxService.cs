using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;

namespace ErpManagement.Services.IServices.Transactions;

public interface ICashboxService
{
    Task<Response<CashboxGetAllResponse>> GetAllCashboxesAsync(RequestLangEnum lang, PaginationRequest model, int? branchId);
    Task<Response<PaginatedCashboxesData>> CreateCashboxAsync(CashboxCreateRequest model);
    Task<Response<CashboxShiftGetByIdResponse>> OpenShiftAsync(CashboxShiftOpenRequest model, string userId);
    Task<Response<CashboxShiftGetByIdResponse>> CloseShiftAsync(CashboxShiftCloseRequest model);
    Task<Response<CashMovementDto>> AddMovementAsync(CashMovementCreateRequest model, string userId);
    Task<Response<CashboxShiftGetByIdResponse>> GetShiftByIdAsync(int shiftId);
    Task<Response<CashLedgerResponse>> GetCashLedgerAsync(int branchId, DateTime from, DateTime to);
}
