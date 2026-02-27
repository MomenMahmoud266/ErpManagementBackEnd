using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Gym;

public class MembershipPlan : TenantEntity
{
    public int BranchId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public int DurationDays { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int ProductId { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Branch Branch { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public virtual ICollection<MemberSubscription> Subscriptions { get; set; } = new HashSet<MemberSubscription>();
}
