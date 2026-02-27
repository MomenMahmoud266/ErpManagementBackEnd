using ErpManagement.Domain.DTOs.Request;
using ErpManagement.Domain.DTOs.Request.Shared.Gym;
using ErpManagement.Domain.DTOs.Response.Shared.Gym;
using ErpManagement.Domain.DTOs.Response.Transactions;

namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GymController(IMembershipService service) : ControllerBase
{
    private readonly IMembershipService _service = service;

    [HttpGet(ApiRoutes.Gym.GetAllPlans)]
    [Produces(typeof(Response<MembershipPlanGetAllResponse>))]
    public async Task<IActionResult> GetAllPlansAsync([FromQuery] PaginationRequest model, [FromQuery] int? branchId)
    {
        var response = await _service.GetAllPlansAsync(GetCurrentRequestLanguage(), model, branchId);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Gym.CreatePlan)]
    [Produces(typeof(Response<MembershipPlanCreateRequest>))]
    public async Task<IActionResult> CreatePlanAsync(MembershipPlanCreateRequest model)
    {
        var response = await _service.CreatePlanAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPut(ApiRoutes.Gym.UpdatePlan)]
    [Produces(typeof(Response<MembershipPlanUpdateRequest>))]
    public async Task<IActionResult> UpdatePlanAsync([FromRoute] int id, MembershipPlanUpdateRequest model)
    {
        var response = await _service.UpdatePlanAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpDelete(ApiRoutes.Gym.DeletePlan)]
    [Produces(typeof(Response<object>))]
    public async Task<IActionResult> DeletePlanAsync([FromRoute] int id)
    {
        var response = await _service.DeletePlanAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Gym.PurchaseMembership)]
    [Produces(typeof(Response<SaleGetByIdResponse>))]
    public async Task<IActionResult> PurchaseMembershipAsync(PurchaseMembershipRequest model)
    {
        var response = await _service.PurchaseMembershipAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [HttpPost(ApiRoutes.Gym.CheckIn)]
    [Produces(typeof(Response<CheckInResponse>))]
    public async Task<IActionResult> CheckInAsync(CheckInRequest model)
    {
        var response = await _service.CheckInAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        return StatusCode(StatusCodes.Status400BadRequest, response);
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
