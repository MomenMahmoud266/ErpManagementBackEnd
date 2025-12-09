namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class ExpenseInvoiceGetByIdResponse
{
    public int Id { get; set; }
    public string ExpenseCode { get; set; } = string.Empty;
    public DateTime ExpenseDate { get; set; }
    public string? CategoryName { get; set; }
    public string? SupplierName { get; set; }
    public string? BranchName { get; set; }
    public string? WarehouseName { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Description { get; set; }
}