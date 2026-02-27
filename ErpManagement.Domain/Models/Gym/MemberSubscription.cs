using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Models.Gym;

public class MemberSubscription : TenantEntity
{
    public int BranchId { get; set; }
    public int CustomerId { get; set; }
    public int MembershipPlanId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public int? LastSaleId { get; set; }

    public virtual Branch Branch { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
    public virtual MembershipPlan MembershipPlan { get; set; } = null!;
    public virtual Sale? LastSale { get; set; }
    public virtual ICollection<MemberCheckIn> CheckIns { get; set; } = new HashSet<MemberCheckIn>();
}
