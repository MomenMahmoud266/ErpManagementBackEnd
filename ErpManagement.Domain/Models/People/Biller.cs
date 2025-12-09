using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Auth;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Models.People;

public class Biller : BaseEntity
{
    public int TenantId { get; set; }
    public string? UserId { get; set; }
    public int WarehouseId { get; set; }
    public int CountryId { get; set; }

    [Required]
    [MaxLength(50)]
    public string BillerCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(20)]
    public string? ZipCode { get; set; }

    [MaxLength(100)]
    public string? NidPassportNumber { get; set; }

    public DateTime? DateOfJoin { get; set; }

    [MaxLength(500)]
    public string? ImagePath { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ApplicationUser? User { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual Country Country { get; set; } = null!;
    
    public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
}