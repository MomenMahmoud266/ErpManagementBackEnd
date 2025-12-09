using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Auth;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Domain.Models.Inventory;

namespace ErpManagement.Domain.Models.Core;

/// <summary>
/// Multi-tenant shop/supermarket entity
/// </summary>
public class Tenant : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string? ContactEmail { get; set; }

    [MaxLength(50)]
    public string? ContactPhone { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
    public virtual ICollection<Branch> Branches { get; set; } = new HashSet<Branch>();
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    public virtual ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
    public virtual ICollection<Supplier> Suppliers { get; set; } = new HashSet<Supplier>();
    public virtual ICollection<Brand> Brands { get; set; } = new HashSet<Brand>();
    public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    public virtual ICollection<ProductType> ProductTypes { get; set; } = new HashSet<ProductType>();
    public virtual ICollection<Unit> Units { get; set; } = new HashSet<Unit>();
    public virtual ICollection<Variant> Variants { get; set; } = new HashSet<Variant>();
    public virtual ICollection<Tax> Taxes { get; set; } = new HashSet<Tax>();
    public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    public virtual ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new HashSet<StockMovement>();
}