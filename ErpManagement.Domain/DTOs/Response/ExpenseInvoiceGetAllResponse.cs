namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class ExpenseInvoiceGetAllResponse
{
    public int TotalRecords { get; set; }
    public List<PaginatedExpenseInvoicesData> Items { get; set; } = [];
}