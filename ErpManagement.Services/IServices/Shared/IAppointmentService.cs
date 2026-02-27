using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Shared.Appointment;
using ErpManagement.Domain.DTOs.Response.Shared.Appointment;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;

namespace ErpManagement.Services.IServices.Shared;

public interface IAppointmentService
{
    Task<Response<AppointmentGetAllResponse>> GetAllAsync(RequestLangEnum lang, AppointmentGetAllFiltrationsRequest model);
    Task<Response<AppointmentCreateRequest>> CreateAsync(AppointmentCreateRequest model);
    Task<Response<AppointmentGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<AppointmentUpdateRequest>> UpdateAsync(int id, AppointmentUpdateRequest model);
    Task<Response<object>> DeleteAsync(int id);
    Task<Response<SaleGetByIdResponse>> CompleteAndInvoiceAsync(int appointmentId, AppointmentCompleteInvoiceRequest model);
}
