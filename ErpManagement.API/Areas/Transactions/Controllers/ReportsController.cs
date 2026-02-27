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
public class ReportsController(IReportsService service) : ControllerBase
{
    private readonly IReportsService _service = service;

    [HttpPost(ApiRoutes.Reports.ProfitLoss)]
    [Produces(typeof(Response<ProfitLossResponse>))]
    public async Task<IActionResult> ProfitLossAsync([FromBody] ProfitLossRequest model)
    {
        var response = await _service.GetProfitLossAsync(GetCurrentRequestLanguage(), model);
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
