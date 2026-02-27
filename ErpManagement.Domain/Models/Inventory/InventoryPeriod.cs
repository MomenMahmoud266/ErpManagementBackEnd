using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Inventory;

/// <summary>
/// Represents an inventory accounting period used for periodic (جرد دوري) costing.
/// </summary>
public class InventoryPeriod : TenantEntity
{
    public int BranchId { get; set; }

    public DateTime From { get; set; }
    public DateTime To { get; set; }

    public bool IsClosed { get; set; } = false;
    public DateTime? ClosedAt { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BeginningValue { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PurchasesValue { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal EndingValue { get; set; } = 0;

    /// <summary>
    /// COGS = BeginningValue + PurchasesValue - EndingValue
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal CogsValue { get; set; } = 0;

    // Navigation
    public virtual Branch Branch { get; set; } = null!;
    public virtual ICollection<PhysicalCount> PhysicalCounts { get; set; } = new HashSet<PhysicalCount>();
}
