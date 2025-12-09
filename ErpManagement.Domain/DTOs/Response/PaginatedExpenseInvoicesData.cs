// ErpManagement.Domain\DTOs\Response\Transactions\PaginatedExpenseInvoicesData.cs
namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PaginatedExpenseInvoicesData : SelectListMoreResponse
{
    public int Id { get; set; }
    public string? InvoiceCode { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string? CategoryName { get; set; }
    public string? SupplierName { get; set; }
    public string? BranchName { get; set; }
    public string? WarehouseName { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public bool IsActive { get; set; }
}