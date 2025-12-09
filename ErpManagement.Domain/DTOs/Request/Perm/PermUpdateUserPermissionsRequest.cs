namespace ErpManagement.Domain.Dtos.Request.Perm;

public class PermUpdateUserPermissionsRequest
{
    public required string UserId { get; set; }
    public List<ActionsWithClaimValue> ActionsWithClaimValues { get; set; } = [];
}
