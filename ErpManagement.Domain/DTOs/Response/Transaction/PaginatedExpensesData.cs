namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PaginatedExpensesData : SelectListMoreResponse
{
    public int Id { get; set; }
    public string? ExpenseCode { get; set; }
    public int ExpenseCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? SupplierName { get; set; }
    public DateTime ExpenseDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}