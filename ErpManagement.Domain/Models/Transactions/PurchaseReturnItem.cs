using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.Models.Transactions;

public class PurchaseReturnItem : BaseEntity
{
    public int PurchaseReturnId { get; set; }
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    // Navigation properties
    public virtual PurchaseReturn PurchaseReturn { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}