using ErpManagement.Domain.DTOs.Request.Purchases;
using ErpManagement.Domain.DTOs.Response.Purchases;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Route("api/[controller]")]
public class PurchasesController(IPurchaseService service) : ControllerBase
{
    private readonly IPurchaseService _service = service;

    [HttpGet("GetAllPurchases")]
    [Produces(typeof(Response<PurchaseGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] PurchaseGetAllFiltrationsForPurchasesRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetPurchaseById/{id:int}")]
    [Produces(typeof(Response<PurchaseGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPost("CreatePurchase")]
    public async Task<IActionResult> CreateAsync(PurchaseCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut("UpdatePurchase/{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, PurchaseUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch("UpdateActiveOrNotPurchase/{id:int}")]
    public async Task<IActionResult> UpdateActiveOrNotAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete("DeletePurchase/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
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