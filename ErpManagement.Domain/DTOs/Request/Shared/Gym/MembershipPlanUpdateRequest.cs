namespace ErpManagement.Domain.DTOs.Request.Shared.Gym;

public class MembershipPlanUpdateRequest
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
