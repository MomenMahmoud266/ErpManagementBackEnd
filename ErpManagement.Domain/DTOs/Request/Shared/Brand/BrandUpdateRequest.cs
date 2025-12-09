namespace ErpManagement.Domain.DTOs.Request.Shared.Brand;

public class BrandUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = Annotations.Title), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = Annotations.Description)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? Description { get; set; }

    [Display(Name = Annotations.ImagePath)]
    [MaxLength(300, ErrorMessage = Annotations.MaxLengthIs300)]
    public string? ImagePath { get; set; }
}