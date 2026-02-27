namespace ErpManagement.Domain.DTOs.Response.Shared.Gym;

public class PaginatedMembershipPlansData
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public int ProductId { get; set; }
    public bool IsActive { get; set; }
}
