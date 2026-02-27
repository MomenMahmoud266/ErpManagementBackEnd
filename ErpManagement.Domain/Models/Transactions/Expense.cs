using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Core;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Auth;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Transactions;

public class Expense : BaseEntity, ITenantEntity
{
    public int TenantId { get; set; }
    public string? UserId { get; set; }
    public int? WarehouseId { get; set; }
    public int? SupplierId { get; set; }
    public int ExpenseCategoryId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ExpenseCode { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    [MaxLength(100)]
    public string? VoucherNo { get; set; }

    [MaxLength(50)]
    public string ExpenseType { get; set; } = "Direct";

    [MaxLength(1000)]
    public string? Comment { get; set; }

    [MaxLength(500)]
    public string? AttachmentPath { get; set; }

    // Navigation properties
    public virtual Tenant Tenant { get; set; } = null!;
    public virtual ApplicationUser? User { get; set; }
    public virtual Warehouse? Warehouse { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual ExpenseCategory ExpenseCategory { get; set; } = null!;
}