using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;

namespace ErpManagement.Domain.Models.Organization;

public class Department : TenantEntity
{
    public int? BranchId { get; set; }

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
    public virtual Branch? Branch { get; set; }
}