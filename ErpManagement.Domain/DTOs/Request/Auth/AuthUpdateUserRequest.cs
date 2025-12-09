namespace ErpManagement.Domain.Dtos.Request.Auth;

public class AuthUpdateUserRequest
{
    public string Id { get; set; } = string.Empty;

    public string? UserName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? CompanyEmail { get; set; }
}
