using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.API.Areas.Transactions.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExpenseCategoriesController(IExpenseCategoryService service) : ControllerBase
{
    private readonly IExpenseCategoryService _service = service;

    [HttpGet("ListOfExpenseCategories")]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet("GetAllExpenseCategories")]
    [Produces(typeof(Response<ExpenseCategoryGetAllResponse>))]
    public async Task<IActionResult> GetAllAsync([FromQuery] ExpenseCategoryGetAllFiltrationsForExpenseCategoriesRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost("CreateExpenseCategory")]
    public async Task<IActionResult> CreateAsync(ExpenseCategoryCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet("GetExpenseCategoryById/{id:int}")]
    [Produces(typeof(Response<ExpenseCategoryGetByIdResponse>))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut("UpdateExpenseCategory/{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, ExpenseCategoryUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch("ChangeActiveOrNotExpenseCategory/{id:int}")]
    public async Task<IActionResult> UpdateActiveOrNotAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete("DeleteExpenseCategory/{id:int}")]
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