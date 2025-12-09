namespace ErpManagement.Domain.Dtos.Response.Perm;

public class PermGetPermissionsToSpecificRoleResponse
{
    public required string RoleId { get; set; } 
    public required string RoleName { get; set; } 
    public List<GetEachSubMainWithActions> EachSubMainWithActions { get; set; } = [];
}
