// ErpManagement.API\Areas\Transactions\Controllers\PurchaseReturnsController.cs
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Route("api/[controller]")]
public class PurchaseReturnsController(IPurchaseReturnService service) : ControllerBase
{
    private readonly IPurchaseReturnService _service = service;

    [HttpGet("GetAllPurchaseReturns")]
    [Produces(typeof(Response<PurchaseReturnGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] PurchaseReturnGetAllFiltrationsForPurchaseReturnsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetPurchaseReturnById/{id:int}")]
    [Produces(typeof(Response<PurchaseReturnGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPost("CreatePurchaseReturn")]
    public async Task<IActionResult> CreateAsync(PurchaseReturnCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut("UpdatePurchaseReturn/{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, PurchaseReturnUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch("ChangeActiveOrNotPurchaseReturn/{id:int}")]
    public async Task<IActionResult> UpdateActiveOrNotAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete("DeletePurchaseReturn/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        // reuse the existing helper from base controllers in this project
        var langHeader = HttpContext.Request.Headers["lang"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(langHeader)) return RequestLangEnum.En;
        return Enum.TryParse<RequestLangEnum>(langHeader, ignoreCase: true, out var lang) ? lang : RequestLangEnum.En;
    }
}