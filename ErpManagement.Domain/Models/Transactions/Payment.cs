using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Core;

namespace ErpManagement.Domain.Models.Transactions;

public class Payment : TenantEntity
{
    public int? PurchaseId { get; set; }
    public int? SaleId { get; set; }

    [MaxLength(50)]
    public string PaymentCode { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PayableAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PaidAmount { get; set; }

    public DateTime PaymentDate { get; set; }

    [MaxLength(50)]
    public string PaymentType { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? AccountNumber { get; set; }

    [MaxLength(100)]
    public string? TransactionNumber { get; set; }

    [MaxLength(1000)]
    public string? Remark { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Purchase? Purchase { get; set; }
    public Sale? Sale { get; set; }
}