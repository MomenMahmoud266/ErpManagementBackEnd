using ErpManagement.Domain.DTOs.Request;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CashboxesController(ICashboxService service) : ControllerBase
{
    private readonly ICashboxService _service = service;

    [HttpGet(ApiRoutes.Cashbox.GetAll)]
    [Produces(typeof(Response<CashboxGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] PaginationRequest model,
        [FromQuery] int? branchId)
    {
        var response = await _service.GetAllCashboxesAsync(GetCurrentRequestLanguage(), model, branchId);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Cashbox.Create)]
    [Produces(typeof(Response<PaginatedCashboxesData>))]
    public async Task<IActionResult> CreateAsync(CashboxCreateRequest model)
    {
        var response = await _service.CreateCashboxAsync(model);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Cashbox.OpenShift)]
    [Produces(typeof(Response<CashboxShiftGetByIdResponse>))]
    public async Task<IActionResult> OpenShiftAsync(CashboxShiftOpenRequest model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(SDStatic.RequestClaims.UserId)
                     ?? string.Empty;
        var response = await _service.OpenShiftAsync(model, userId);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Cashbox.CloseShift)]
    [Produces(typeof(Response<CashboxShiftGetByIdResponse>))]
    public async Task<IActionResult> CloseShiftAsync(CashboxShiftCloseRequest model)
    {
        var response = await _service.CloseShiftAsync(model);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Cashbox.AddMovement)]
    [Produces(typeof(Response<CashMovementDto>))]
    public async Task<IActionResult> AddMovementAsync(CashMovementCreateRequest model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(SDStatic.RequestClaims.UserId)
                     ?? string.Empty;
        var response = await _service.AddMovementAsync(model, userId);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpGet(ApiRoutes.Cashbox.ShiftById)]
    [Produces(typeof(Response<CashboxShiftGetByIdResponse>))]
    public async Task<IActionResult> GetShiftByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetShiftByIdAsync(id);
        return response.IsSuccess ? Ok(response) : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpGet(ApiRoutes.Cashbox.Ledger)]
    [Produces(typeof(Response<CashLedgerResponse>))]
    public async Task<IActionResult> GetLedgerAsync(
        [FromQuery] int branchId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        var response = await _service.GetCashLedgerAsync(branchId, from, to);
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
