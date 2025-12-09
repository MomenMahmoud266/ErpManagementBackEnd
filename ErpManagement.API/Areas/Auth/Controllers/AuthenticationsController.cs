namespace ErpManagement.API.Areas.Auth.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[ApiController]
[Authorize(PermissionsStatic.Auth.View)]
[Route("Api/[controller]")]
public class AuthenticationsController(IAuthenticationService service) : ControllerBase
{
    private readonly IAuthenticationService _service = service;

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.LoginUser)]
    [Produces(typeof(Response<AuthLoginUserResponse>))]
    public async Task<IActionResult> LoginUserAsync(AuthLoginUserRequest model)
    {
        var response = await _service.LoginUserAsync(model);
        if (response.IsSuccess)
        {
            //if (!string.IsNullOrEmpty(response.Data.RefreshToken))
            //    SetRefreshTokenInCookie(response.Data.RefreshToken, response.Data.RefreshTokenExpiration);
            return Ok(response);
        }
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [AllowAnonymous]
    [HttpDelete(ApiRoutes.User.LogOutUser)]
    [Produces(typeof(Response<string>))]
    public async Task<IActionResult> LogOutUserAsync()
    {
        var response = await _service.LogOutUserAsync();
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.User.UpdateUser)]
    [Produces(typeof(Response<AuthUpdateUserRequest>))]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] string id, AuthUpdateUserRequest model)
    {
        var response = await _service.UpdateUserAsync(id, model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.User.ShowPasswordToSpecificUser)]
    [Produces(typeof(Response<string>))]
    public async Task<IActionResult> ShowPasswordToSpecificUserAsync([FromRoute] string id)
    {
        var response = await _service.ShowPasswordToSpecificUserAsync(id);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.User.SetNewPasswordToSpecificUser)]
    [Produces(typeof(Response<AuthSetNewPasswordRequest>))]
    public async Task<IActionResult> SetNewPasswordToSpecificUserAsync(AuthSetNewPasswordRequest model)
    {
        var response = await _service.SetNewPasswordToSpecificUserAsync(model);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.User.SetNewPasswordToSuperAdmin)]
    [Produces(typeof(Response<string>))]
    public async Task<IActionResult> SetNewPasswordToSuperAdminAsync([FromRoute] string newPassword)
    {
        var response = await _service.SetNewPasswordToSuperAdminAsync(newPassword);
        if (response.IsSuccess)
            return Ok(response);
        else if (!response.IsSuccess)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime(),
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
