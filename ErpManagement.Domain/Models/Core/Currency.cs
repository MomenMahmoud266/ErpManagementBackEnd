using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.Models.Core;

public class Currency : BaseEntity
{
    [MaxLength(10)]
    public string Code { get; set; } = "EGP";      // ISO 4217

    [MaxLength(10)]
    public string Symbol { get; set; } = "E£";

    public int DecimalDigits { get; set; } = 2;

    public bool IsActive { get; set; } = true;

    public virtual ICollection<Tenant> Tenants { get; set; } = new HashSet<Tenant>();
}
