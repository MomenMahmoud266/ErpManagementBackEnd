// ErpManagement.Domain\DTOs\Request\Transactions\ExpenseInvoiceGetAllFiltrationsForExpenseInvoicesRequest.cs
namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class ExpenseInvoiceGetAllFiltrationsForExpenseInvoicesRequest : PaginationRequest
{
    public int? ExpenseCategoryId { get; set; }
    public int? SupplierId { get; set; }
    public int? BranchId { get; set; }
    public int? WarehouseId { get; set; }
    public string? ExpenseCode { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? IsActive { get; set; }
}