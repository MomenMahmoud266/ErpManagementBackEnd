namespace ErpManagement.Domain.DTOs.Request.Shared;

public class ProductTypeUpdateRequest
{
    [Display(Name = Annotations.Title), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string Title { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    [Display(Name = Annotations.Type)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string Type { get; set; } = "Product";

    [Display(Name = Annotations.Description)]
    [MaxLength(1000, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? Description { get; set; }

    [Required]
    public int Id { get; set; }
}