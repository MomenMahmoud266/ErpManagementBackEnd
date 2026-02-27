using ErpManagement.Domain.DTOs.Request.Shared.Variant;
using ErpManagement.Domain.DTOs.Response.Shared.Variant;

namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(Modules.Product)]
[ApiExplorerSettings(GroupName = Modules.Product)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VariantsController(IVariantService service) : ControllerBase
{
    private readonly IVariantService _service = service;

    [HttpGet(ApiRoutes.Variant.ListOfVariants)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfVariantsAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Variant.GetAllVariants)]
    [Produces(typeof(Response<VariantGetAllResponse>))]
    public async Task<IActionResult> GetAllVariantsAsync([FromQuery] VariantGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Variant.CreateVariant)]
    public async Task<IActionResult> CreateVariantAsync(VariantCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Variant.GetVariantById)]
    [Produces(typeof(Response<VariantGetByIdResponse>))]
    public async Task<IActionResult> GetVariantByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Variant.UpdateVariant)]
    public async Task<IActionResult> UpdateVariantAsync([FromRoute] int id, VariantUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Variant.UpdateActiveOrNotVariant)]
    public async Task<IActionResult> UpdateActiveOrNotVariantAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Variant.DeleteVariant)]
    public async Task<IActionResult> DeleteVariantAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    // helper to mirror Brand controller pattern
    private RequestLangEnum GetCurrentRequestLanguage()
    {
        var lang = (Request.Headers["Accept-Language"].ToString() ?? string.Empty).ToLower();
        return lang.Contains("ar") ? RequestLangEnum.Ar : lang.Contains("tr") ? RequestLangEnum.Tr : RequestLangEnum.En;
    }
}