using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.Models.Inventory;

/// <summary>
/// Event-sourced inventory movements for full audit trail
/// </summary>
public class StockMovement : TenantEntity
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }

    [Required]
    [MaxLength(50)]
    public string MovementType { get; set; } = string.Empty;
    // "Purchase", "Sale", "PurchaseReturn", "SaleReturn", "Adjustment", "Transfer"

    [Required]
    [MaxLength(50)]
    public string Direction { get; set; } = string.Empty; // "In", "Out"

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceAfter { get; set; }

    public DateTime MovementDate { get; set; }

    [MaxLength(50)]
    public string? ReferenceType { get; set; }

    public int? ReferenceId { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public virtual Warehouse Warehouse { get; set; } = null!;
}