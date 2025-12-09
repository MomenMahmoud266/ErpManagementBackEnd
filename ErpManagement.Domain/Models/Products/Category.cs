using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Models.Products;

public class Category : TenantEntity
{
    public int? ParentId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty; // "Product", "Expense"

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImagePath { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category> Children { get; set; } = new HashSet<Category>();
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    public virtual ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
    public virtual ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
}