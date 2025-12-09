namespace ErpManagement.Domain.Dtos.Request.Auth;

public class AuthLoginUserRequest
{
    [Display(Name = Annotations.UserName), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public required string UserName { get; set; } 

    [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
    [Display(Name = Annotations.Password), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public required string Password { get; set; } 

    [Display(Name = Annotations.RememberMe)]
    public bool RememberMe { get; set; }

    //public string? DeviceId { get; set; }
}
