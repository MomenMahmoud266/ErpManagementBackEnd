using ErpManagement.Domain.DTOs.Request.People.Biller;
using ErpManagement.Domain.DTOs.Response.People.Biller;
using ErpManagement.Services.IServices.People;

namespace ErpManagement.API.Areas.People.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class BillersController(IBillerService service) : ControllerBase
{
    private readonly IBillerService _service = service;

    [HttpGet(ApiRoutes.Biller.ListOfBillers)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfBillersAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Biller.GetAllBillers)]
    [Produces(typeof(Response<BillerGetAllResponse>))]
    public async Task<IActionResult> GetAllBillersAsync([FromQuery] BillerGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Biller.CreateBiller)]
    public async Task<IActionResult> CreateBillerAsync(BillerCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Biller.GetBillerById)]
    [Produces(typeof(Response<BillerGetByIdResponse>))]
    public async Task<IActionResult> GetBillerByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Biller.UpdateBiller)]
    public async Task<IActionResult> UpdateBillerAsync([FromRoute] int id, BillerUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Biller.ChangeActiveOrNotBiller)]
    public async Task<IActionResult> UpdateActiveOrNotBillerAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Biller.DeleteBiller)]
    public async Task<IActionResult> DeleteBillerAsync([FromRoute] int id)
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