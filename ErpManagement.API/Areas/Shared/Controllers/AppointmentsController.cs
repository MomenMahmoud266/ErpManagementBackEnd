using ErpManagement.Domain.DTOs.Request.Shared.Appointment;
using ErpManagement.Domain.DTOs.Response.Shared.Appointment;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(IAppointmentService service) : ControllerBase
{
    private readonly IAppointmentService _service = service;

    [HttpGet(ApiRoutes.Appointment.GetAll)]
    [Produces(typeof(Response<AppointmentGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] AppointmentGetAllFiltrationsRequest model)
    {
        var response = await _service.GetAllAsync(GetCurrentRequestLanguage(), model);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Appointment.Create)]
    [Produces(typeof(Response<AppointmentCreateRequest>))]
    public async Task<IActionResult> CreateAsync(AppointmentCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpGet(ApiRoutes.Appointment.GetById)]
    [Produces(typeof(Response<AppointmentGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPut(ApiRoutes.Appointment.Update)]
    [Produces(typeof(Response<AppointmentUpdateRequest>))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, AppointmentUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpDelete(ApiRoutes.Appointment.Delete)]
    [Produces(typeof(Response<object>))]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Appointment.CompleteAndInvoice)]
    [Produces(typeof(Response<SaleGetByIdResponse>))]
    public async Task<IActionResult> CompleteAndInvoiceAsync([FromRoute] int id, AppointmentCompleteInvoiceRequest model)
    {
        var response = await _service.CompleteAndInvoiceAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        string lang = Request.Headers.AcceptLanguage.ToString();
        if (lang.StartsWith(RequestLang.Ar))
            return RequestLangEnum.Ar;
        else if (lang.StartsWith(RequestLang.En))
            return RequestLangEnum.En;
        else
            return RequestLangEnum.Tr;
    }
}
