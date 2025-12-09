namespace ErpManagement.Domain.Dtos.Request.Perm;

public class PermCreateRoleRequest
{
    [Display(Name = Annotations.NameAr), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = Annotations.NameEn), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public string NameAr { get; set; } = string.Empty;

    [Display(Name = Annotations.NameTr), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public string NameTr { get; set; } = string.Empty;
}

