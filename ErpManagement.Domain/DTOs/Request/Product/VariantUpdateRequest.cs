namespace ErpManagement.Domain.DTOs.Request.Shared.Variant;

public class VariantUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = Annotations.Type), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string VariantType { get; set; } = string.Empty;

    [Display(Name = Annotations.Name), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(100, ErrorMessage = Annotations.MaxLengthIs100)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = Annotations.Code)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string? Code { get; set; }
}