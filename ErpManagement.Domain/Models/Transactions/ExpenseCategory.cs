using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ErpManagement.Domain.Models.Core;

namespace ErpManagement.Domain.Models.Transactions;

public class ExpenseCategory : TenantEntity
{
    [MaxLength(150)]
    public string NameAr { get; set; } = string.Empty;

    [MaxLength(150)]
    public string NameEn { get; set; } = string.Empty;

    [MaxLength(150)]
    public string NameTr { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Expense>? Expenses { get; set; }
}