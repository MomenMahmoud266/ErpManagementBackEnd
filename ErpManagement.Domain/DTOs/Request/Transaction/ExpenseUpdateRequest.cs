using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class ExpenseUpdateRequest : ExpenseCreateRequest
{
    [Required]
    public int Id { get; set; }
}