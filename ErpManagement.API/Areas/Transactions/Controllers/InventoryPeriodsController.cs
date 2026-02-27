using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;
using Microsoft.AspNetCore.Http;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InventoryPeriodsController(IInventoryPeriodService service) : ControllerBase
{
    private readonly IInventoryPeriodService _service = service;

    [HttpGet(ApiRoutes.InventoryPeriods.GetAll)]
    [Produces(typeof(Response<InventoryPeriodGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] int? branchId)
    {
        var response = await _service.GetAllAsync(branchId);
        return Ok(response);
    }

    [HttpGet(ApiRoutes.InventoryPeriods.GetById)]
    [Produces(typeof(Response<InventoryPeriodGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }

    [HttpPost(ApiRoutes.InventoryPeriods.Create)]
    [Produces(typeof(Response<InventoryPeriodGetByIdResponse>))]
    public async Task<IActionResult> CreateAsync([FromBody] InventoryPeriodCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        return response.IsSuccess
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.InventoryPeriods.AddPhysicalCount)]
    [Produces(typeof(Response<InventoryPeriodGetByIdResponse>))]
    public async Task<IActionResult> AddPhysicalCountAsync(
        [FromRoute] int periodId,
        [FromBody] PhysicalCountCreateRequest model)
    {
        var response = await _service.AddPhysicalCountAsync(periodId, model);
        return response.IsSuccess
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.InventoryPeriods.Close)]
    [Produces(typeof(Response<InventoryPeriodGetByIdResponse>))]
    public async Task<IActionResult> CloseAsync([FromRoute] int id)
    {
        var response = await _service.CloseAsync(id);
        return response.IsSuccess
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpDelete(ApiRoutes.InventoryPeriods.Delete)]
    [Produces(typeof(Response<string>))]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }
}
