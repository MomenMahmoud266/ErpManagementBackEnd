using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Transactions;

public class CashboxShift : TenantEntity
{
    public int CashboxId { get; set; }

    [Required]
    [MaxLength(450)]
    public string OpenedByUserId { get; set; } = string.Empty;

    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal OpeningCash { get; set; }

    public DateTime? ClosedAt { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ClosingCash { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ExpectedCash { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Difference { get; set; }

    public bool IsClosed { get; set; } = false;

    public virtual Cashbox Cashbox { get; set; } = null!;
    public virtual ICollection<CashMovement> Movements { get; set; } = new HashSet<CashMovement>();
}
