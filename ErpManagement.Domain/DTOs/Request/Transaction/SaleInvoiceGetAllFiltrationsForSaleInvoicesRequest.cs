// ErpManagement.Domain\DTOs\Request\Transactions\SaleInvoiceGetAllFiltrationsForSaleInvoicesRequest.cs
namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class SaleInvoiceGetAllFiltrationsForSaleInvoicesRequest : PaginationRequest
{
    public int? CustomerId { get; set; }
    public int? WarehouseId { get; set; }
    public int? BillerId { get; set; }
    public string? SaleCode { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? IsActive { get; set; }
}