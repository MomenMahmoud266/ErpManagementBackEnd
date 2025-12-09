using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;

namespace ErpManagement.Domain.Models.Products;

public class Tax : TenantEntity
{

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}