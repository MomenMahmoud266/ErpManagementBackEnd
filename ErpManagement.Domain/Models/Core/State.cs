using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Core;

public class State : BaseEntity
{
    public int CountryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string NameEn { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? NameAr { get; set; }

    [MaxLength(100)]
    public string? NameTr { get; set; }

    [MaxLength(10)]
    public string? Code { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Country Country { get; set; } = null!;
}