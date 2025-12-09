// ErpManagement.Domain\DTOs\Response\Transactions\PaginatedSaleInvoicesData.cs
namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PaginatedSaleInvoicesData : SelectListMoreResponse
{
    public int Id { get; set; }
    public string? InvoiceCode { get; set; }
    public DateTime SaleDate { get; set; }
    public string? CustomerName { get; set; }
    public string? WarehouseName { get; set; }
    public string? BillerName { get; set; }
    public decimal TotalAmount { get; set; }
    public string SaleStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}