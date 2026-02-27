using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;

namespace ErpManagement.Services.IServices.Transactions;

public interface IReportsService
{
    Task<Response<ProfitLossResponse>> GetProfitLossAsync(RequestLangEnum lang, ProfitLossRequest model);
}
