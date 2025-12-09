using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Auth;

namespace ErpManagement.Domain.Models.Transactions;

public class PurchaseReturn : BaseEntity
{
    public int TenantId { get; set; }
    public string? UserId { get; set; }
    public int SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public int? PurchaseId { get; set; }
    public virtual Purchase? Purchase { get; set; }
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

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ApplicationUser? User { get; set; }
    public virtual Supplier Supplier { get; set; } = null!;
    public virtual Warehouse? Warehouse { get; set; }
    
    public virtual ICollection<PurchaseReturnItem> Items { get; set; } = new HashSet<PurchaseReturnItem>();
}