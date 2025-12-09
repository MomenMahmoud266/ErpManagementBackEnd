namespace ErpManagement.Domain.Dtos.Request.Auth;

public class AuthChangePassOfUserRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
