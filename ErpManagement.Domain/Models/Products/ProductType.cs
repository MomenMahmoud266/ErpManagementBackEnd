using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;

namespace ErpManagement.Domain.Models.Products;

public class ProductType : TenantEntity
{
    public int? ParentId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Type { get; set; } = "Product";

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ProductType? Parent { get; set; }
    public virtual ICollection<ProductType> Children { get; set; } = new HashSet<ProductType>();
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}