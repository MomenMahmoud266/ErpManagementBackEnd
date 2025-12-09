using ErpManagement.Domain.DTOs.Request.Shared;
using ErpManagement.Domain.DTOs.Response.Shared;
using ErpManagement.Services.IServices.Products;

namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class ProductTypesController(IProductTypeService service) : ControllerBase
{
    private readonly IProductTypeService _service = service;

    [HttpGet(ApiRoutes.ProductType.ListOfProductTypes)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfProductTypesAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.ProductType.GetAllProductTypes)]
    [Produces(typeof(Response<ProductTypeGetAllResponse>))]
    public async Task<IActionResult> GetAllProductTypesAsync([FromQuery] ProductTypeGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.ProductType.CreateProductType)]
    public async Task<IActionResult> CreateProductTypeAsync(ProductTypeCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.ProductType.GetProductTypeById)]
    [Produces(typeof(Response<ProductTypeGetByIdResponse>))]
    public async Task<IActionResult> GetProductTypeByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.ProductType.UpdateProductType)]
    public async Task<IActionResult> UpdateProductTypeAsync([FromRoute] int id, ProductTypeUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.ProductType.UpdateActiveOrNotProductType)]
    public async Task<IActionResult> UpdateActiveOrNotProductTypeAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.ProductType.DeleteProductType)]
    public async Task<IActionResult> DeleteProductTypeAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    // Language helper copied from README pattern
    private RequestLangEnum GetCurrentRequestLanguage()
    {
        string lang = Request.Headers.AcceptLanguage.ToString();
        if (lang.StartsWith(RequestLang.Ar)) return RequestLangEnum.Ar;
        else if (lang.StartsWith(RequestLang.En)) return RequestLangEnum.En;
        else return RequestLangEnum.Tr;
    }
}