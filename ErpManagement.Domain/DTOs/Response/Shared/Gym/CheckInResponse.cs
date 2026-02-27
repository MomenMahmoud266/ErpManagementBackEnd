using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Response.Shared.Gym;

public class CheckInResponse
{
    public int MemberSubscriptionId { get; set; }
    public int CheckInId { get; set; }
    public DateTime CheckInAt { get; set; }
    public SubscriptionStatus SubscriptionStatus { get; set; }
    public DateTime SubscriptionEndAt { get; set; }
}
