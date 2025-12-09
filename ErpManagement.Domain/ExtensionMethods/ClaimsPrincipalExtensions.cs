namespace ErpManagement.Domain.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(RequestClaims.UserId)!;

    public static string GetRoleId(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Role)!;

    public static string GetRoleName(this ClaimsPrincipal principal) =>
        principal.Claims.Where(a => a.Type == ClaimTypes.Role).FirstOrDefault()!.Value;

    public static List<string> GetRolesNames(this ClaimsPrincipal principal) =>
        principal.Claims.Where(a => a.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

    //public static string GetRoleName(this ClaimsPrincipal principal) =>
    //    principal.Claims.Where(a => a.Type == ClaimTypes.Role).FirstOrDefault()!.Value;
}
