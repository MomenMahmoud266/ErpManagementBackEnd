using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Auth;

namespace ErpManagement.Domain.Models.Core;

public class Gender : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string NameEn { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? NameAr { get; set; }

    [MaxLength(50)]
    public string? NameTr { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
}