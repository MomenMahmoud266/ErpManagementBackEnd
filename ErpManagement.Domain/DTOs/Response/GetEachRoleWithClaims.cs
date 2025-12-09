namespace ErpManagement.Domain.Dtos.Response;

public class GetEachRoleWithClaims
{
    public string RoleName { get; set; } = string.Empty;
    public IList<Claim> Claims = [];
}
