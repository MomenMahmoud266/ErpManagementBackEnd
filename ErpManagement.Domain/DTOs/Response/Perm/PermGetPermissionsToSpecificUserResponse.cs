namespace ErpManagement.Domain.Dtos.Response.Perm;

public class PermGetPermissionsToSpecificUserResponse
{
    public required string UserId { get; set; } 
    public required string UserName { get; set; } 
    public List<GetEachSubMainWithActions> EachSubMainWithActions { get; set; } = [];
}
