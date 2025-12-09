// ErpManagement.Domain\DTOs\Response\Transactions\PaginatedPurchaseInvoicesData.cs
namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PaginatedPurchaseInvoicesData : SelectListMoreResponse
{
    public int Id { get; set; }
    public string? InvoiceCode { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string? SupplierName { get; set; }
    public string? WarehouseName { get; set; }
    public decimal TotalAmount { get; set; }
    public string PurchaseStatus { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}