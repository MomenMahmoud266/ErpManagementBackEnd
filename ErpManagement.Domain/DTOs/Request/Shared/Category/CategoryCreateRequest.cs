namespace ErpManagement.Domain.DTOs.Request.Shared.Category;

public class CategoryCreateRequest
{
    public int? ParentId { get; set; }

    [Display(Name = Annotations.Title), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = Annotations.Type), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string Type { get; set; } = string.Empty;

    [Display(Name = Annotations.Description)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? Description { get; set; }

    [Display(Name = Annotations.ImagePath)]
    [MaxLength(300, ErrorMessage = Annotations.MaxLengthIs300)]
    public string? ImagePath { get; set; }
}