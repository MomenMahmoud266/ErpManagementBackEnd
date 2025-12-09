using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.Models.Transactions;

public class StockTransferItem : TenantEntity
{
    public int StockTransferId { get; set; }
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [MaxLength(500)]
    public string? Remark { get; set; }

    // Navigation
    public StockTransfer StockTransfer { get; set; } = null!;
    public Product Product { get; set; } = null!;
}