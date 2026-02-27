using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController(ISaleService service) : ControllerBase
{
    private readonly ISaleService _service = service;

    [HttpGet(ApiRoutes.Sale.GetAll)]
    [Produces(typeof(Response<SaleGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] SaleGetAllFiltrationsForSalesRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet(ApiRoutes.Sale.GetById)]
    [Produces(typeof(Response<SaleGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPost(ApiRoutes.Sale.Create)]
    [Produces(typeof(Response<SaleGetByIdResponse>))]
    public async Task<IActionResult> CreateAsync(SaleCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Sale.Update)]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, SaleUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Sale.ChangeActiveOrNot)]
    public async Task<IActionResult> UpdateActiveOrNotAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Sale.Delete)]
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