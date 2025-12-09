using ErpManagement.Domain.DTOs.Request.Shared;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class PaymentCreateRequest
{
    [Display(Name = Annotations.Purchase)]
    public int? PurchaseId { get; set; }

    [Display(Name = Annotations.Sale)]
    public int? SaleId { get; set; }

    [MaxLength(50)]
    public string? PaymentCode { get; set; }

    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public decimal PaidAmount { get; set; }

    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public decimal PayableAmount { get; set; }

    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public DateTime PaymentDate { get; set; }

    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(50)]
    public string PaymentType { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? AccountNumber { get; set; }

    [MaxLength(100)]
    public string? TransactionNumber { get; set; }

    [MaxLength(1000)]
    public string? Remark { get; set; }
}