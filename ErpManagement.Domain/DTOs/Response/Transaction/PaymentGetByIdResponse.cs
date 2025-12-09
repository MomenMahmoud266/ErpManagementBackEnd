using ErpManagement.Domain.DTOs.Response.Shared;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PaymentGetByIdResponse : SelectListMoreResponse
{
    public int Id { get; set; }
    public string? PaymentCode { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public decimal PaidAmount { get; set; }
    public decimal PayableAmount { get; set; }
    public string? AccountNumber { get; set; }
    public string? TransactionNumber { get; set; }
    public string? Remark { get; set; }

    public int? PurchaseId { get; set; }
    public string? PurchaseCode { get; set; }

    public int? SaleId { get; set; }
    public string? SaleCode { get; set; }

    public bool IsActive { get; set; }
}