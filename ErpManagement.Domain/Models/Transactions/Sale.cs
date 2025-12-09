using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Auth;

namespace ErpManagement.Domain.Models.Transactions;

public class Sale : BaseEntity
{
    public int TenantId { get; set; }
    public string? UserId { get; set; }
    public int CustomerId { get; set; }
    public int? WarehouseId { get; set; }
    public int? BillerId { get; set; }

    [Required]
    [MaxLength(50)]
    public string SaleCode { get; set; } = string.Empty;

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

    public DateTime SaleDate { get; set; }

    [MaxLength(50)]
    public string SaleStatus { get; set; } = "Pending";

    [MaxLength(50)]
    public string PaymentStatus { get; set; } = "Unpaid";

    [MaxLength(1000)]
    public string? Note { get; set; }

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ApplicationUser? User { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public virtual Warehouse? Warehouse { get; set; }
    public virtual Biller? Biller { get; set; }
    
    public virtual ICollection<SaleItem> Items { get; set; } = new HashSet<SaleItem>();
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}