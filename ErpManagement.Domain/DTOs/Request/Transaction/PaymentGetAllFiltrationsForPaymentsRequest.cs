using ErpManagement.Domain.DTOs.Request.Shared;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class PaymentGetAllFiltrationsForPaymentsRequest : PaginationRequest
{
    public int? SaleId { get; set; }
    public int? PurchaseId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? PaymentType { get; set; }
}