using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Transactions;

public class Cashbox : TenantEntity
{
    public int BranchId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "Main Cashbox";

    public new bool IsActive { get; set; } = true;

    public virtual Branch Branch { get; set; } = null!;
    public virtual ICollection<CashboxShift> Shifts { get; set; } = new HashSet<CashboxShift>();
}
