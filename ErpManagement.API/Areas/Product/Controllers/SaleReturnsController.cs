using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SaleReturnsController(ISaleReturnService service) : ControllerBase
{
    private readonly ISaleReturnService _service = service;

    [HttpGet("GetAllSaleReturns")]
    [Produces(typeof(Response<SaleReturnGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] SaleReturnGetAllFiltrationsForSaleReturnsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetSaleReturnById/{id:int}")]
    [Produces(typeof(Response<SaleReturnGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPost("CreateSaleReturn")]
    public async Task<IActionResult> CreateAsync(SaleReturnCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut("UpdateSaleReturn/{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, SaleReturnUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch("ChangeActiveOrNotSaleReturn/{id:int}")]
    public async Task<IActionResult> UpdateActiveOrNotAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete("DeleteSaleReturn/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    // helper to get current request language like other controllers
    private RequestLangEnum GetCurrentRequestLanguage()
    {
        var lang = HttpContext?.Request?.Headers["Accept-Language"].ToString();
        return string.Equals(lang, "ar", StringComparison.OrdinalIgnoreCase) ? RequestLangEnum.Ar : RequestLangEnum.En;
    }
}