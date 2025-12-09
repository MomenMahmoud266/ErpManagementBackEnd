using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Auth;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Models.People;

public class Customer : TenantEntity
{
    public string? UserId { get; set; }
    public int CountryId { get; set; }
    public int? CategoryId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CustomerCode { get; set; } = string.Empty;

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

    [Column(TypeName = "decimal(18,2)")]
    public decimal RewardPoints { get; set; }

    [MaxLength(500)]
    public string? ImagePath { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ApplicationUser? User { get; set; }
    public virtual Country Country { get; set; } = null!;
    public virtual Category? Category { get; set; }
    
    public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    public virtual ICollection<SaleReturn> SaleReturns { get; set; } = new HashSet<SaleReturn>();
}