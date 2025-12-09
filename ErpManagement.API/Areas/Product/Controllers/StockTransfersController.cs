using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Response.Transaction;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Product.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Route("api/[controller]")]
public class StockTransfersController : ControllerBase
{
    private readonly IStockTransferService _service;

    public StockTransfersController(IStockTransferService service)
    {
        _service = service;
    }

    [HttpGet("GetAllStockTransfers")]
    [Produces(typeof(Response<StockTransferGetAllResponse>))]
    public async Task<IActionResult> GetAllStockTransfersAsync([FromQuery] StockTransferGetAllFiltrationsForStockTransfersRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetStockTransferById/{id:int}")]
    [Produces(typeof(Response<StockTransferGetByIdResponse>))]
    public async Task<IActionResult> GetStockTransferByIdAsync([FromRoute] int id)
    {
        var resp = await _service.GetByIdAsync(id);
        if (resp.IsSuccess) return Ok(resp);
        else if (!resp.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, resp);
        return StatusCode(StatusCodes.Status500InternalServerError, resp);
    }

    [HttpPost("CreateStockTransfer")]
    public async Task<IActionResult> CreateStockTransferAsync(StockTransferCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut("UpdateStockTransfer/{id:int}")]
    public async Task<IActionResult> UpdateStockTransferAsync([FromRoute] int id, StockTransferUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch("ChangeActiveOrNotStockTransfer/{id:int}")]
    public async Task<IActionResult> ChangeActiveOrNotStockTransferAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete("DeleteStockTransfer/{id:int}")]
    public async Task<IActionResult> DeleteStockTransferAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    // Reuse helper from other controllers
    private RequestLangEnum GetCurrentRequestLanguage() =>
        HttpContext.Request.Headers.ContainsKey("x-lang") && HttpContext.Request.Headers["x-lang"].ToString().ToLower() == "ar"
            ? RequestLangEnum.Ar
            : RequestLangEnum.En;
}