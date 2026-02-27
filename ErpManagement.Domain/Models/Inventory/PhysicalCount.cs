using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Inventory;

/// <summary>
/// Physical count line for a specific product in a warehouse at period close.
/// </summary>
public class PhysicalCount : TenantEntity
{
    public int InventoryPeriodId { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CountQty { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CostUsed { get; set; }

    /// <summary>
    /// LineValue = CountQty * CostUsed
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal LineValue { get; set; }

    // Navigation
    public virtual InventoryPeriod InventoryPeriod { get; set; } = null!;
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
