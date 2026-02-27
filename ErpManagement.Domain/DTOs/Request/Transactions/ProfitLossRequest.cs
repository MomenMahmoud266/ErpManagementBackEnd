using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class ProfitLossRequest
{
    [Required]
    public DateTime From { get; set; }

    [Required]
    public DateTime To { get; set; }

    /// <summary>Filter by specific branch. Null = all branches.</summary>
    public int? BranchId { get; set; }

    /// <summary>
    /// true  = include all sales regardless of payment status (revenue = TotalAmount)
    /// false = include only fully paid sales (revenue = sum of Payments.PaidAmount)
    /// </summary>
    public bool IncludeUnpaidSales { get; set; } = true;
}
