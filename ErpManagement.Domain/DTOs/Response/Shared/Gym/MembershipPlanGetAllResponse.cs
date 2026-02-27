namespace ErpManagement.Domain.DTOs.Response.Shared.Gym;

public class MembershipPlanGetAllResponse
{
    public int TotalRecords { get; set; }
    public List<PaginatedMembershipPlansData> Items { get; set; } = new();
}
