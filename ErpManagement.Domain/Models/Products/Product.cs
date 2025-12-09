using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Models.Products;

public class Product : TenantEntity
{
    public int CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public int? UnitId { get; set; }
    public int? TaxId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Cost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Tax { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? AlertQuantity { get; set; }

    [MaxLength(500)]
    public string? ImagePath { get; set; }

    [MaxLength(100)]
    public string? Barcode { get; set; }

    [MaxLength(100)]
    public string? SKU { get; set; }

    public bool IsFeatured { get; set; }
    public bool IsExpired { get; set; }
    public bool IsPromoSale { get; set; }

    public DateTime? ExpiryDate { get; set; }
    public DateTime? ManufactureDate { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? ColorVariantIds { get; set; }

    [MaxLength(500)]
    public string? SizeVariantIds { get; set; }

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;
    public virtual Supplier? Supplier { get; set; }
    public virtual Brand? Brand { get; set; }
    public virtual ProductType? Type { get; set; }
    public virtual Unit? Unit { get; set; }
    public virtual Tax? TaxNavigation { get; set; }
    
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new HashSet<StockMovement>();
    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new HashSet<PurchaseItem>();
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new HashSet<SaleItem>();
}