using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Core;

/// <summary>
/// Generic definition/lookup table for various categorizations
/// </summary>
public class Definition : BaseEntity
{
    public int TenantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Type { get; set; } = string.Empty; // "ExpenseCategory", "PaymentType"

    [Required]
    [MaxLength(200)]
    public string NameEn { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? NameAr { get; set; }

    [MaxLength(200)]
    public string? NameTr { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
}