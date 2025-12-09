using ErpManagement.Domain.DTOs.Response.Shared;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PaginatedPaymentsData : SelectListMoreResponse
{
    public int Id { get; set; }
    public string? PaymentCode { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal PayableAmount { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public string? SaleCode { get; set; }
    public string? PurchaseCode { get; set; }
    public bool IsActive { get; set; }
}