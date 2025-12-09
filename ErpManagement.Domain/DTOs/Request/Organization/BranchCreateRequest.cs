using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Organization.Branch;

public class BranchCreateRequest
{
    [Required]
    [MaxLength(200)]
    [Display(Name = "Name in English")]
    public string NameEn { get; set; } = string.Empty;

    [MaxLength(200)]
    [Display(Name = "Name in Arabic")]
    public string? NameAr { get; set; }

    [MaxLength(200)]
    [Display(Name = "Name in Turkish")]
    public string? NameTr { get; set; }

    [Required]
    [Display(Name = "Country")]
    public int CountryId { get; set; }

    [MaxLength(100)]
    [Display(Name = "City")]
    public string? City { get; set; }

    [MaxLength(20)]
    [Display(Name = "Zip Code")]
    public string? ZipCode { get; set; }

    [MaxLength(500)]
    [Display(Name = "Address")]
    public string? Address { get; set; }

    [MaxLength(50)]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }
}