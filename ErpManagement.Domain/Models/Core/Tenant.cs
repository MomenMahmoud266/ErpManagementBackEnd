using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Enums;
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

    // ── Subscription / Trial fields ──────────────────────────────────
    public DateTime? TrialEndsAt { get; set; }
    public DateTime? SubscriptionEndsAt { get; set; }
    public bool IsSubscriptionActive { get; set; } = true;

    // Computed helper (not mapped to DB)
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public bool IsAccessAllowed =>
        IsActive &&
        (IsSubscriptionActive || (SubscriptionEndsAt.HasValue && SubscriptionEndsAt >= DateTime.UtcNow)) &&
        (!TrialEndsAt.HasValue || TrialEndsAt >= DateTime.UtcNow || IsSubscriptionActive);

    public BusinessType BusinessType { get; set; } = BusinessType.Retail;

    public bool EnableInventory { get; set; } = true;
    public bool EnableAppointments { get; set; } = false;
    public bool EnableMemberships { get; set; } = false;
    public bool EnableTables { get; set; } = false;
    public bool EnableKitchenRouting { get; set; } = false;

    // Inventory costing settings
    [MaxLength(20)]
    public string InventoryMode { get; set; } = "Perpetual"; // "Perpetual" or "Periodic"
    [MaxLength(20)]
    public string CostingMethod { get; set; } = "Average"; // "Average" (MVP), future: "FIFO"

    // International / localization settings
    public int CurrencyId { get; set; } = 1;        // FK to Currencies (default EGP)
    [MaxLength(10)]
    public string CountryCode { get; set; } = "EG";     // ISO 3166-1 alpha-2
    [MaxLength(100)]
    public string TimeZoneId { get; set; } = "Africa/Cairo";
    [MaxLength(50)]
    public string TaxLabel { get; set; } = "VAT";

    // Navigation properties
    public virtual Currency Currency { get; set; } = null!;
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