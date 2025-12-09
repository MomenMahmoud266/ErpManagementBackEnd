using ErpManagement.Domain.DTOs.Request.People.Customer;
using ErpManagement.Domain.DTOs.Response.People.Customer;

namespace ErpManagement.API.Areas.People.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService service) : ControllerBase
{
    private readonly ICustomerService _service = service;

    [HttpGet(ApiRoutes.Customer.ListOfCustomers)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfCustomersAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Customer.GetAllCustomers)]
    [Produces(typeof(Response<CustomerGetAllResponse>))]
    public async Task<IActionResult> GetAllCustomersAsync([FromQuery] CustomerGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Customer.CreateCustomer)]
    public async Task<IActionResult> CreateCustomerAsync(CustomerCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Customer.GetCustomerById)]
    [Produces(typeof(Response<CustomerGetByIdResponse>))]
    public async Task<IActionResult> GetCustomerByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Customer.UpdateCustomer)]
    public async Task<IActionResult> UpdateCustomerAsync([FromRoute] int id, CustomerUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Customer.ChangeActiveOrNotCustomer)]
    public async Task<IActionResult> UpdateActiveOrNotCustomerAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Customer.DeleteCustomer)]
    public async Task<IActionResult> DeleteCustomerAsync([FromRoute] int id)
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