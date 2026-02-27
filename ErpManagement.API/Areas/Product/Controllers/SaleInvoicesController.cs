// ErpManagement.API.Areas.Transactions.Controllers\SaleInvoicesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SaleInvoicesController(ISaleInvoiceService service) : ControllerBase
{
    private readonly ISaleInvoiceService _service = service;

    [HttpGet("GetAllSaleInvoices")]
    [Produces(typeof(Response<SaleInvoiceGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] SaleInvoiceGetAllFiltrationsForSaleInvoicesRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetSaleInvoiceById/{id:int}")]
    [Produces(typeof(Response<SaleInvoiceGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        // reuse pattern from other controllers in the project
        var acceptLang = Request.Headers["Accept-Language"].ToString();
        return string.IsNullOrEmpty(acceptLang) ? RequestLangEnum.En : Enum.TryParse<RequestLangEnum>(acceptLang, true, out var res) ? res : RequestLangEnum.En;
    }
}