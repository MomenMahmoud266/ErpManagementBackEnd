using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Gym;

public class MemberCheckIn : TenantEntity
{
    public int BranchId { get; set; }
    public int CustomerId { get; set; }
    public int MemberSubscriptionId { get; set; }
    public DateTime CheckInAt { get; set; } = DateTime.UtcNow;

    public virtual Branch Branch { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
    public virtual MemberSubscription MemberSubscription { get; set; } = null!;
}
