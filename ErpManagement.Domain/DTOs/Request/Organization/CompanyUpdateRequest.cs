namespace ErpManagement.Domain.DTOs.Request.Organization.Company;

public class CompanyUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = Annotations.Company), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = Annotations.Description)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? Description { get; set; }

    [Display(Name = Annotations.Email)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string? Email { get; set; }

    [Display(Name = Annotations.Phone)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string? Phone { get; set; }

    [Display(Name = Annotations.Address)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? Address { get; set; }
}