using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class CashboxShiftCloseRequest
{
    [Required]
    public int ShiftId { get; set; }

    [Required]
    public decimal ClosingCash { get; set; }
}
