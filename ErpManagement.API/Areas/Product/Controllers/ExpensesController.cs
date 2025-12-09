using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Route("api/[controller]")]
public class ExpensesController(IExpenseService service) : ControllerBase
{
    private readonly IExpenseService _service = service;

    [HttpGet("GetAllExpenses")]
    [Produces(typeof(Response<ExpenseGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] ExpenseGetAllFiltrationsForExpensesRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpGet("GetExpenseById/{id:int}")]
    [Produces(typeof(Response<ExpenseGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPost("CreateExpense")]
    public async Task<IActionResult> CreateAsync(ExpenseCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut("UpdateExpense/{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, ExpenseUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch("ChangeActiveOrNotExpense/{id:int}")]
    public async Task<IActionResult> UpdateActiveOrNotAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete("DeleteExpense/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
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