using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IPaymentService service) : ControllerBase
{
    private readonly IPaymentService _service = service;

    [HttpGet("GetAllPayments")]
    [Produces(typeof(Response<PaymentGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaymentGetAllFiltrationsForPaymentsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetPaymentById/{id:int}")]
    [Produces(typeof(Response<PaymentGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPost("CreatePayment")]
    public async Task<IActionResult> CreateAsync(PaymentCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut("UpdatePayment/{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, PaymentUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch("ChangeActiveOrNotPayment/{id:int}")]
    public async Task<IActionResult> UpdateActiveOrNotAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete("DeletePayment/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        var langHeader = HttpContext.Request.Headers["lang"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(langHeader)) return RequestLangEnum.En;
        return Enum.TryParse<RequestLangEnum>(langHeader, ignoreCase: true, out var lang) ? lang : RequestLangEnum.En;
    }
}