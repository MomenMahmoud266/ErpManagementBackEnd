namespace ErpManagement.Domain.DTOs.Request.Shared.Gym;

public class CheckInRequest
{
    public int BranchId { get; set; }
    public int CustomerId { get; set; }
    public int? MembershipPlanId { get; set; }
}
