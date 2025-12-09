namespace ErpManagement.Domain.Dtos.Response.Perm;

public class PermGetManagementModelResponse
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<CheckBox> ListOfCheckBoxes { get; set; } = [];
}
