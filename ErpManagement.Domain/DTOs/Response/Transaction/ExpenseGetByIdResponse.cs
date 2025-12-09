namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class ExpenseGetByIdResponse
{
    public int Id { get; set; }
    public int ExpenseCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }

    public int? BranchId { get; set; }
    public string? BranchName { get; set; }

    public int? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }

    public string? ExpenseCode { get; set; }
    public DateTime ExpenseDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}