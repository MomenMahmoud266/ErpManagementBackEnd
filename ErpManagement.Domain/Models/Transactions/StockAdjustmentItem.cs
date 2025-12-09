using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.Models.Transactions;

public class StockAdjustmentItem : TenantEntity
{
    public int StockAdjustmentId { get; set; }
    public int ProductId { get; set; }

    // Signed delta:
    //  > 0 => increase stock
    //  < 0 => decrease stock
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [MaxLength(500)]
    public string? Remark { get; set; }

    // Navigation
    public StockAdjustment StockAdjustment { get; set; } = null!;
    public Product Product { get; set; } = null!;
}