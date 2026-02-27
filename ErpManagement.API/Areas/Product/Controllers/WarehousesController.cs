using ErpManagement.Domain.DTOs.Request.Organization.Warehouse;
using ErpManagement.Domain.DTOs.Response.Organization.Warehouse;
using ErpManagement.Services.IServices.Organization;

namespace ErpManagement.API.Areas.Organization.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WarehousesController(IWarehouseService service) : ControllerBase
{
    private readonly IWarehouseService _service = service;

    [HttpGet(ApiRoutes.Warehouse.ListOfWarehouses)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfWarehousesAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Warehouse.GetAllWarehouses)]
    [Produces(typeof(Response<WarehouseGetAllResponse>))]
    public async Task<IActionResult> GetAllWarehousesAsync([FromQuery] WarehouseGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Warehouse.CreateWarehouse)]
    [Produces(typeof(Response<WarehouseCreateRequest>))]
    public async Task<IActionResult> CreateWarehouseAsync(WarehouseCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Warehouse.GetWarehouseById)]
    [Produces(typeof(Response<WarehouseGetByIdResponse>))]
    public async Task<IActionResult> GetWarehouseByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Warehouse.UpdateWarehouse)]
    [Produces(typeof(Response<WarehouseUpdateRequest>))]
    public async Task<IActionResult> UpdateWarehouseAsync([FromRoute] int id, WarehouseUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Warehouse.UpdateActiveOrNotWarehouse)]
    [Produces(typeof(Response<string>))]
    public async Task<IActionResult> UpdateActiveOrNotWarehouseAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Warehouse.DeleteWarehouse)]
    [Produces(typeof(Response<string>))]
    public async Task<IActionResult> DeleteWarehouseAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        string lang = Request.Headers.AcceptLanguage.ToString();
        if (lang.StartsWith(RequestLang.Ar)) return RequestLangEnum.Ar;
        else if (lang.StartsWith(RequestLang.En)) return RequestLangEnum.En;
        else return RequestLangEnum.Tr;
    }
}