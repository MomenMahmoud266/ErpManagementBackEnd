using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Models.Auth;

public class ApplicationUser : IdentityUser
{
    public int TenantId { get; set; }

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(500)]
    public string? ProfileImagePath { get; set; }

    public int? GenderId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? InsertDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }

    public string? InsertBy { get; set; }
    public string? UpdateBy { get; set; }
    public string? DeleteBy { get; set; }
    public string? EmailConfirmToken { get; set; }

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual Gender? Gender { get; set; }
    
    public virtual ICollection<ApplicationUserDevice> Devices { get; set; } = new HashSet<ApplicationUserDevice>();
    
    // Add relationship navigations for UserId foreign keys
    public virtual ICollection<Biller> Billers { get; set; } = new HashSet<Biller>();
    public virtual ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
    public virtual ICollection<Supplier> Suppliers { get; set; } = new HashSet<Supplier>();
    public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new HashSet<PurchaseReturn>();
    public virtual ICollection<SaleReturn> SaleReturns { get; set; } = new HashSet<SaleReturn>();
    public virtual ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
}
