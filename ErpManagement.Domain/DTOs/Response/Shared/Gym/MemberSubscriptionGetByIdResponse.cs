using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Response.Shared.Gym;

public class MemberSubscriptionGetByIdResponse
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int MembershipPlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public SubscriptionStatus Status { get; set; }
    public int? LastSaleId { get; set; }
    public string? LastSaleCode { get; set; }
}
