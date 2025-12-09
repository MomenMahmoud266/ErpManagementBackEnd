namespace ErpManagement.Domain.DTOs.Request.Shared.Unit;

public class UnitUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = Annotations.Name), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(100, ErrorMessage = Annotations.MaxLengthIs100)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = Annotations.UnitType), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string UnitType { get; set; } = string.Empty;

    [Display(Name = Annotations.Symbol)]
    [MaxLength(10, ErrorMessage = Annotations.MaxLengthIs10)]
    public string? Symbol { get; set; }
}