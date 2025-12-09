namespace ErpManagement.Domain.Dtos.Request.Perm;

public class PermUpdateRolePermissionsRequest
{
    public string RoleId { get; set; } = string.Empty;
    public List<CheckBox> ListOfCheckBoxes { get; set; } = [];
}
