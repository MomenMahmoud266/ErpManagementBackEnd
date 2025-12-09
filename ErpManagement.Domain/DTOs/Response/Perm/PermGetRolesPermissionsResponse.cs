namespace ErpManagement.Domain.Dtos.Response.Perm;

public class PermGetRolesPermissionsResponse
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    //public List<RolesPermissionsData> ListOfCheckBoxes { get; set; } = [];
    public List<CheckBox> ListOfCheckBoxes { get; set; } = [];

}

public class RolesPermissionsData : CheckBox
{
    public string RoleClaimName { get; set; } = string.Empty;  
}