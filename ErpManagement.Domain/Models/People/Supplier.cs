using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Domain.Models.Auth;

namespace ErpManagement.Domain.Models.People;

public class Supplier : TenantEntity
{
    public string? UserId { get; set; }
    public int CountryId { get; set; }
    public int? CompanyId { get; set; }

    [Required]
    [MaxLength(50)]
    public string SupplierCode { get; set; } = string.Empty;

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

    [MaxLength(500)]
    public string? ImagePath { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ApplicationUser? User { get; set; }
    public virtual Country Country { get; set; } = null!;
    public virtual Company? Company { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new HashSet<PurchaseReturn>();
    public virtual ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
}