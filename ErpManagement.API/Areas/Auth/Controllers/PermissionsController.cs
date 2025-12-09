namespace ErpManagement.API.Areas.Auth.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Authorize(PermissionsStatic.Auth.View)]
[Route("Api/[controller]")]
public class PermissionsController(IPermissionService service) : ControllerBase
{
    private readonly IPermissionService _service = service;

    [HttpGet(ApiRoutes.Perm.GetAllRoles)]
    [Produces(typeof(Response<IEnumerable<SelectListForUserResponse>>))]
    public async Task<IActionResult> GetAllRolesAsync() =>
        Ok(await _service.GetAllRolesAsync(GetCurrentRequestLanguage()));

    [HttpPost(ApiRoutes.Perm.CreateRole)]
    public async Task<IActionResult> CreateRoleAsync(PermCreateRoleRequest model)
    {
        var response = await _service.CreateRoleAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.Perm.UpdateRole)]
    public async Task<IActionResult> UpdateRoleAsync([FromRoute] string id, PermUpdateRoleRequest model)
    {
        var response = await _service.UpdateRoleAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpDelete(ApiRoutes.Perm.DeleteRole)]
    public async Task<IActionResult> DeleteRoleAsync([FromRoute] string id)
    {
        var response = await _service.DeleteRoleAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Perm.GetEachUserWithHisRoles)]
    public async Task<IActionResult> GetEachUserWithHisRolesAsync([FromQuery] PermGetEachUserWithRolesRequest model)
    {
        var response = await _service.GetEachUserWithHisRolesAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.Perm.GetUserRoles)]
    public async Task<IActionResult> GetUserRolesAsync([FromRoute] string userId)
    {
        var response = await _service.GetUserRolesAsync(userId, GetCurrentRequestLanguage());
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPost(ApiRoutes.Perm.UpdateUserRoles)]
    public async Task<IActionResult> UpdateUserRolesAsync(PermGetManagementModelResponse model)
    {
        var response = await _service.UpdateUserRolesAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }


    [HttpGet(ApiRoutes.Perm.GetRolePermissions)]
    public async Task<IActionResult> GetRolePermissionsAsync([FromRoute] string roleId)
    {
        var response = await _service.GetRolePermissionsAsync(roleId, GetCurrentRequestLanguage());
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPost(ApiRoutes.Perm.UpdateRolePermissions)]
    public async Task<IActionResult> UpdateRolePermissionsAsync(PermUpdateRolePermissionsRequest model)
    {
        var response = await _service.UpdateRolePermissionsAsync(model);
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
