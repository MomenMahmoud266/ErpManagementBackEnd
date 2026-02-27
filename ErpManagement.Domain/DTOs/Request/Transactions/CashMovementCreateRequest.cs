using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class CashMovementCreateRequest
{
    [Required]
    public int ShiftId { get; set; }

    [Required]
    public CashMovementType Type { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;
}
