using ErpManagement.Domain.DTOs.Request.Organization.Branch;
using ErpManagement.Domain.DTOs.Response.Organization.Branch;
using ErpManagement.Services.IServices.Organization;

namespace ErpManagement.API.Areas.Organization.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BranchesController(IBranchService service) : ControllerBase
{
    private readonly IBranchService _service = service;

    [HttpGet(ApiRoutes.Branch.ListOfBranches)]
    [Produces(typeof(Response<IEnumerable<SelectListResponse>>))]
    public async Task<IActionResult> ListOfBranchesAsync() =>
        Ok(await _service.ListAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Branch.GetAllBranches)]
    [Produces(typeof(Response<BranchGetAllResponse>))]
    public async Task<IActionResult> GetAllBranchesAsync([FromQuery] BranchGetAllFiltrationsRequest model) =>
        Ok(await _service.GetAllAsync(GetCurrentRequestLanguage(), model));

    [HttpPost(ApiRoutes.Branch.CreateBranch)]
    [Produces(typeof(Response<BranchCreateRequest>))]
    public async Task<IActionResult> CreateBranchAsync(BranchCreateRequest model)
    {
        var response = await _service.CreateAsync(model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Branch.GetBranchById)]
    [Produces(typeof(Response<BranchGetByIdResponse>))]
    public async Task<IActionResult> GetBranchByIdAsync([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Branch.UpdateBranch)]
    [Produces(typeof(Response<BranchUpdateRequest>))]
    public async Task<IActionResult> UpdateBranchAsync([FromRoute] int id, BranchUpdateRequest model)
    {
        var response = await _service.UpdateAsync(id, model);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    [HttpPatch(ApiRoutes.Branch.ChangeActiveOrNotBranch)]
    [Produces(typeof(Response<string>))]
    public async Task<IActionResult> UpdateActiveOrNotBranchAsync([FromRoute] int id) =>
        Ok(await _service.UpdateActiveOrNotAsync(id));

    [HttpDelete(ApiRoutes.Branch.DeleteBranch)]
    [Produces(typeof(Response<string>))]
    public async Task<IActionResult> DeleteBranchAsync([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        if (response.IsSuccess) return Ok(response);
        else if (!response.IsSuccess) return StatusCode(StatusCodes.Status400BadRequest, response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    private RequestLangEnum GetCurrentRequestLanguage()
    {
        string lang = Request.Headers.AcceptLanguage.ToString();
        if (lang.StartsWith(RequestLang.Ar)) return RequestLangEnum.Ar;
        else if (lang.StartsWith(RequestLang.En)) return RequestLangEnum.En;
        else return RequestLangEnum.Tr;
    }
}