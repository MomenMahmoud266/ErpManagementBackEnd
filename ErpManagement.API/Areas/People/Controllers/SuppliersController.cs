using ErpManagement.Domain.DTOs.Request.People.Supplier;
using ErpManagement.Domain.DTOs.Response.People.Supplier;
using ErpManagement.Services.IServices.People;

namespace ErpManagement.API.Areas.People.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class SuppliersController(ISupplierService service) : ControllerBase
{
    private readonly ISupplierService _service = service;

    [HttpGet(ApiRoutes.Supplier.ListOfSuppliers)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfSuppliersAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Supplier.GetAllSuppliers)]
    [Produces(typeof(Response<SupplierGetAllResponse>))]
    public async Task<IActionResult> GetAllSuppliersAsync([FromQuery] SupplierGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Supplier.CreateSupplier)]
    public async Task<IActionResult> CreateSupplierAsync(SupplierCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Supplier.GetSupplierById)]
    [Produces(typeof(Response<SupplierGetByIdResponse>))]
    public async Task<IActionResult> GetSupplierByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Supplier.UpdateSupplier)]
    public async Task<IActionResult> UpdateSupplierAsync([FromRoute] int id, SupplierUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Supplier.ChangeActiveOrNotSupplier)]
    public async Task<IActionResult> UpdateActiveOrNotSupplierAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Supplier.DeleteSupplier)]
    public async Task<IActionResult> DeleteSupplierAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
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