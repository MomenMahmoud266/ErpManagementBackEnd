using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Auth;

public class ApplicationUserDevice : BaseEntity
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string DeviceToken { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? DeviceType { get; set; } // "iOS", "Android", "Web"

    [MaxLength(200)]
    public string? DeviceName { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime LastUsedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
}
