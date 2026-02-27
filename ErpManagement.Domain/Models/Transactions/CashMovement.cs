using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Transactions;

public class CashMovement : TenantEntity
{
    public int CashboxShiftId { get; set; }

    public CashMovementType Type { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(450)]
    public string CreatedByUserId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ReferenceType { get; set; }

    public int? ReferenceId { get; set; }

    public virtual CashboxShift CashboxShift { get; set; } = null!;
}
