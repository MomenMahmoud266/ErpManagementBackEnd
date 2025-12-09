namespace ErpManagement.Domain.Dtos.Request.Auth;

public class AuthSetNewPasswordRequest
{
    public required string UserId { get; set; } 
    public required string NewPassword { get; set; } 
}
