using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Models.Organization;

public class Warehouse : BaseEntity
{
    public int BranchId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new HashSet<StockMovement>();
    public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    public virtual ICollection<Biller> Billers { get; set; } = new HashSet<Biller>();
    public virtual ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
}