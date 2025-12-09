using ErpManagement.Domain.DTOs.Request.Products;
using ErpManagement.Domain.DTOs.Response.Products;
using ErpManagement.Services.IServices.Products;

namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService service) : ControllerBase
{
    private readonly IProductService _service = service;

    [HttpGet(ApiRoutes.Product.ListOfProducts)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfProductsAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Product.GetAllProducts)]
    [Produces(typeof(Response<ProductGetAllResponse>))]
    public async Task<IActionResult> GetAllProductsAsync([FromQuery] ProductGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Product.CreateProduct)]
    public async Task<IActionResult> CreateProductAsync(ProductCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Product.GetProductById)]
    [Produces(typeof(Response<ProductGetByIdResponse>))]
    public async Task<IActionResult> GetProductByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Product.UpdateProduct)]
    public async Task<IActionResult> UpdateProductAsync([FromRoute] int id, ProductUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Product.UpdateActiveOrNotProduct)]
    public async Task<IActionResult> UpdateActiveOrNotProductAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Product.DeleteProduct)]
    public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
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