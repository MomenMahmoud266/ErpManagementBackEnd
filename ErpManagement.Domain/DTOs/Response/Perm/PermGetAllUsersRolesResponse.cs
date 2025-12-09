namespace ErpManagement.Domain.Dtos.Response.Perm;

public class PermGetAllUsersRolesResponse
{
    public int TotalRecords { get; set; } = 0;
    public List<PermGetAllUsersRoles> Data { get; set; } = new();
}
public class PermGetAllUsersRoles
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CompanyEmail { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
