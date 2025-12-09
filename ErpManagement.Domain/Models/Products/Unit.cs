using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;

namespace ErpManagement.Domain.Models.Products;

public class Unit : TenantEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string UnitType { get; set; } = string.Empty; // "Weight", "Volume", "Piece"

    [MaxLength(20)]
    public string? Symbol { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}