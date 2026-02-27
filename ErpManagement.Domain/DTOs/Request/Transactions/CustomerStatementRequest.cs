using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class CustomerStatementRequest
{
    [Required]
    public int CustomerId { get; set; }

    public int? BranchId { get; set; }

    [Required]
    public DateTime From { get; set; }

    [Required]
    public DateTime To { get; set; }
}
