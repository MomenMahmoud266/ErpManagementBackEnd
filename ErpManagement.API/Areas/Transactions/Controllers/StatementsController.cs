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
public class StatementsController(IStatementsService service) : ControllerBase
{
    private readonly IStatementsService _service = service;

    [HttpPost(ApiRoutes.Statements.Customer)]
    [Produces(typeof(Response<CustomerStatementResponse>))]
    public async Task<IActionResult> CustomerStatementAsync([FromBody] CustomerStatementRequest model)
    {
        var response = await _service.GetCustomerStatementAsync(GetCurrentRequestLanguage(), model);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Statements.Supplier)]
    [Produces(typeof(Response<SupplierStatementResponse>))]
    public async Task<IActionResult> SupplierStatementAsync([FromBody] SupplierStatementRequest model)
    {
        var response = await _service.GetSupplierStatementAsync(GetCurrentRequestLanguage(), model);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        string lang = Request.Headers.AcceptLanguage.ToString();
        if (lang.StartsWith(RequestLang.Ar)) return RequestLangEnum.Ar;
        if (lang.StartsWith(RequestLang.En)) return RequestLangEnum.En;
        return RequestLangEnum.Tr;
    }
}
