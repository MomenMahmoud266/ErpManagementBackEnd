namespace ErpManagement.Domain.Dtos.Request.Auth;

public class AuthRegisterUserRequest
{
    //[MinLength(6), MaxLength(20)]
    public string UserName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Display(Name = Annotations.Password), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = Annotations.ConfirmationPassword), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [Compare(nameof(Password), ErrorMessage = Annotations.ConfirmationPasswordNotMatch)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
