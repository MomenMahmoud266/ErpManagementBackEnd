using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class CashboxShiftOpenRequest
{
    [Required]
    public int CashboxId { get; set; }

    [Required]
    public decimal OpeningCash { get; set; }
}
