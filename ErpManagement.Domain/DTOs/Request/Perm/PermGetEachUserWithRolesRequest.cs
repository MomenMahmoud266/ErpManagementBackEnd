namespace ErpManagement.Domain.Dtos.Request.Perm;

public class PermGetEachUserWithRolesRequest : PaginationRequest
{
    public DateTime? CreationStartDate { get; set; }
    public DateTime? CreationEndDate { get; set; }
}
