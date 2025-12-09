using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Transactions;

public class SaleReturnItem : BaseEntity
{
    public int SaleReturnId { get; set; }
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

    // Navigation
    public virtual SaleReturn? SaleReturn { get; set; }
    public virtual Product? Product { get; set; }
}