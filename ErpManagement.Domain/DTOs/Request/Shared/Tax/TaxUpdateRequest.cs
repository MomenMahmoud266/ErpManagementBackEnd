namespace ErpManagement.Domain.DTOs.Request.Shared.Tax;

public class TaxUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = Annotations.Title), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = Annotations.Amount), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [Range(0, 999999.99, ErrorMessage = Annotations.AmountMustBeValid)]
    public decimal Amount { get; set; }

    [Display(Name = Annotations.Description)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? Description { get; set; }
}