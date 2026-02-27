namespace ErpManagement.Domain.DTOs.Request.Shared.Gym;

public class PurchaseMembershipRequest
{
    public int BranchId { get; set; }
    public int WarehouseId { get; set; }
    public int BillerId { get; set; }
    public int CustomerId { get; set; }
    public int MembershipPlanId { get; set; }
    public decimal PaidAmount { get; set; } = 0;
    public string PaymentType { get; set; } = "Cash";
    public string? TransactionNumber { get; set; }
    public string? AccountNumber { get; set; }
}
