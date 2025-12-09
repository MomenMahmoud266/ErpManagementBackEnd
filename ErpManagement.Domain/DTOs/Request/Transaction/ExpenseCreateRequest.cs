using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class ExpenseCreateRequest
{
    [Required]
    public int ExpenseCategoryId { get; set; }

    public int? SupplierId { get; set; }
    public int? BranchId { get; set; }
    public int? WarehouseId { get; set; }

    [MaxLength(50)]
    public string? ExpenseCode { get; set; }

    [Required]
    public DateTime ExpenseDate { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = Annotations.FieldMustBeGreaterThanZero)]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(100)]
    public string PaymentMethod { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}