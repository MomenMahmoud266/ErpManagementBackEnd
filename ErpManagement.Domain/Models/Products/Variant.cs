using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;

namespace ErpManagement.Domain.Models.Products;

public class Variant : BaseEntity
{
    public int TenantId { get; set; }

    [Required]
    [MaxLength(50)]
    public string VariantType { get; set; } = string.Empty; // "Color", "Size"

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
}