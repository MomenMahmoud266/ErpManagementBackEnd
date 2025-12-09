namespace ErpManagement.Domain.DTOs.Request.Shared.Country;

public class SharUpdateCountryRequest
{
    public int Id { get; set; }

    [Display(Name = Annotations.NameAr), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public string NameAr { get; set; } = string.Empty;

    [Display(Name = Annotations.NameEn), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public string NameEn { get; set; } = string.Empty;

    [Display(Name = Annotations.NameTr), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public string NameTr { get; set; } = string.Empty;
}
