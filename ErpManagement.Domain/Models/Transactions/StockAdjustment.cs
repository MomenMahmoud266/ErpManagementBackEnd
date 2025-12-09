using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Auth;

namespace ErpManagement.Domain.Models.Transactions;

public class StockAdjustment : TenantEntity
{
    public int WarehouseId { get; set; }

    [MaxLength(50)]
    public string AdjustmentCode { get; set; } = string.Empty;

    public DateTime AdjustmentDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalQuantity { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public Warehouse Warehouse { get; set; } = null!;
    public ApplicationUser? User { get; set; }

    public ICollection<StockAdjustmentItem>? Items { get; set; }
}