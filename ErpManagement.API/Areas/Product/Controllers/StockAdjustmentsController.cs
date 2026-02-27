using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Response.Transaction;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Product.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StockAdjustmentsController : ControllerBase
{
    private readonly IStockAdjustmentService _service;

    public StockAdjustmentsController(IStockAdjustmentService service)
    {
        _service = service;
    }

    [HttpGet("GetAllStockAdjustments")]
    [Produces(typeof(Response<StockAdjustmentGetAllResponse>))]
    public async Task<IActionResult> GetAllStockAdjustmentsAsync([FromQuery] StockAdjustmentGetAllFiltrationsForStockAdjustmentsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetStockAdjustmentById/{id:int}")]
    [Produces(typeof(Response<StockAdjustmentGetByIdResponse>))]
    public async Task<IActionResult> GetStockAdjustmentByIdAsync([FromRoute] int id)
    {
        var resp = await _service.GetByIdAsync(id);
        if (resp.IsSuccess) return Ok(resp);
        else if (!resp.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, resp);
        return StatusCode(StatusCodes.Status500InternalServerError, resp);
    }

    [HttpPost("CreateStockAdjustment")]
    public async Task<IActionResult> CreateStockAdjustmentAsync(StockAdjustmentCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut("UpdateStockAdjustment/{id:int}")]
    public async Task<IActionResult> UpdateStockAdjustmentAsync([FromRoute] int id, StockAdjustmentUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch("ChangeActiveOrNotStockAdjustment/{id:int}")]
    public async Task<IActionResult> ChangeActiveOrNotStockAdjustmentAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete("DeleteStockAdjustment/{id:int}")]
    public async Task<IActionResult> DeleteStockAdjustmentAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    // helper methods referenced from other controllers - adjust if you have a shared base controller in project
    private RequestLangEnum GetCurrentRequestLanguage()
    {
        // fallback: default English if real helper not available in this context
        return Request.Headers.ContainsKey("x-lang") && Request.Headers["x-lang"] == "ar" ? RequestLangEnum.Ar : RequestLangEnum.En;
    }
}