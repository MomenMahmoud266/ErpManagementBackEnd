using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Auth;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Products;

namespace ErpManagement.Domain.Models.Transactions;

public class SaleReturn : BaseEntity
{
    public int TenantId { get; set; }
    public string? UserId { get; set; }

    [Required]
    public int CustomerId { get; set; }

    public int? WarehouseId { get; set; }

    public int? BillerId { get; set; }

    // Link to original Sale
    [Required]
    public int SaleId { get; set; }
    public virtual Sale? Sale { get; set; }

    [Required]
    [MaxLength(50)]
    public string ReturnCode { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public DateTime ReturnDate { get; set; }

    [MaxLength(50)]
    public string ReturnStatus { get; set; } = "Pending";

    [MaxLength(500)]
    public string? Remark { get; set; }

    [MaxLength(1000)]
    public string? ReturnNote { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual Customer? Customer { get; set; }
    public virtual Warehouse? Warehouse { get; set; }
    public virtual Biller? Biller { get; set; }
    public virtual ApplicationUser? User { get; set; }

    public virtual ICollection<SaleReturnItem> Items { get; set; } = new HashSet<SaleReturnItem>();
}