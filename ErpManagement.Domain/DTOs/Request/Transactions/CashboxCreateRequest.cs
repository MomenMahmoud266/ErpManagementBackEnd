using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class CashboxCreateRequest
{
    [Required]
    public int BranchId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "Main Cashbox";
}
