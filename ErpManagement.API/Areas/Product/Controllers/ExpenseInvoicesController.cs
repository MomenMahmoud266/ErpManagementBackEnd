// ErpManagement.API.Areas.Transactions.Controllers\ExpenseInvoicesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Route("api/[controller]")]
public class ExpenseInvoicesController(IExpenseInvoiceService service) : ControllerBase
{
    private readonly IExpenseInvoiceService _service = service;

    [HttpGet("GetAllExpenseInvoices")]
    [Produces(typeof(Response<ExpenseInvoiceGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] ExpenseInvoiceGetAllFiltrationsForExpenseInvoicesRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetExpenseInvoiceById/{id:int}")]
    [Produces(typeof(Response<ExpenseInvoiceGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        var acceptLang = Request.Headers["Accept-Language"].ToString();
        return string.IsNullOrEmpty(acceptLang) ? RequestLangEnum.En : Enum.TryParse<RequestLangEnum>(acceptLang, true, out var res) ? res : RequestLangEnum.En;
    }
}