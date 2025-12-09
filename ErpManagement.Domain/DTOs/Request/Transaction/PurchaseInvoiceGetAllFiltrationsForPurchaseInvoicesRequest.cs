// ErpManagement.Domain\DTOs\Request\Transactions\PurchaseInvoiceGetAllFiltrationsForPurchaseInvoicesRequest.cs
namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class PurchaseInvoiceGetAllFiltrationsForPurchaseInvoicesRequest : PaginationRequest
{
    public int? SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public string? PurchaseCode { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? IsActive { get; set; }
}