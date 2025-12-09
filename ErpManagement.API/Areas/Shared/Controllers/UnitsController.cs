namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class UnitsController(IUnitService service) : ControllerBase
{
    private readonly IUnitService _service = service;

    [HttpGet(ApiRoutes.Unit.ListOfUnits)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfUnitsAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Unit.GetAllUnits)]
    [Produces(typeof(Response<UnitGetAllResponse>))]
    public async Task<IActionResult> GetAllUnitsAsync([FromQuery] UnitGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Unit.CreateUnit)]
    public async Task<IActionResult> CreateUnitAsync(UnitCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Unit.GetUnitById)]
    [Produces(typeof(Response<UnitGetByIdResponse>))]
    public async Task<IActionResult> GetUnitByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Unit.UpdateUnit)]
    public async Task<IActionResult> UpdateUnitAsync([FromRoute] int id, UnitUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Unit.UpdateActiveOrNotUnit)]
    public async Task<IActionResult> UpdateActiveOrNotUnitAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Unit.DeleteUnit)]
    public async Task<IActionResult> DeleteUnitAsync([FromRoute] int id)
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