namespace ErpManagement.Domain.DTOs.Request.People.Supplier;

public class SupplierCreateRequest
{
    [Display(Name = Annotations.Country), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int CountryId { get; set; }

    [Display(Name = Annotations.Company)]
    public int? CompanyId { get; set; }

    [Display(Name = Annotations.Code), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string SupplierCode { get; set; } = string.Empty;

    [Display(Name = Annotations.Name), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(100, ErrorMessage = Annotations.MaxLengthIs100)]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = Annotations.Name)]
    [MaxLength(100, ErrorMessage = Annotations.MaxLengthIs100)]
    public string? LastName { get; set; }

    [Display(Name = Annotations.PhoneNumber)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string? Phone { get; set; }

    [Display(Name = Annotations.Email)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string? Email { get; set; }

    [Display(Name = Annotations.Region)]
    [MaxLength(100, ErrorMessage = Annotations.MaxLengthIs100)]
    public string? City { get; set; }

    [Display(Name = Annotations.Description)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? Address { get; set; }

    [Display(Name = Annotations.Code)]
    [MaxLength(20, ErrorMessage = Annotations.MaxLengthIs10)]
    public string? ZipCode { get; set; }

    [Display(Name = Annotations.ImagePath)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? ImagePath { get; set; }
}