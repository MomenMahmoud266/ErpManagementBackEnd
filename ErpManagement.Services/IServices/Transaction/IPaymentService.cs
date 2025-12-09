using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.Services.IServices.Transactions;

public interface IPaymentService
{
    Task<Response<PaymentGetAllResponse>> GetAllAsync(RequestLangEnum lang, PaymentGetAllFiltrationsForPaymentsRequest model);
    Task<Response<PaymentGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<PaymentCreateRequest>> CreateAsync(PaymentCreateRequest model);
    Task<Response<PaymentUpdateRequest>> UpdateAsync(int id, PaymentUpdateRequest model);
    Task<Response<string>> UpdateActiveOrNotAsync(int id);
    Task<Response<string>> DeleteAsync(int id);
}