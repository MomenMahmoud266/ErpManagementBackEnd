namespace ErpManagement.Domain.DTOs.Request.Shared.Gym;

public class MembershipPlanCreateRequest
{
    public int BranchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int? ProductTypeId { get; set; }
    public int? TaxId { get; set; }
}
