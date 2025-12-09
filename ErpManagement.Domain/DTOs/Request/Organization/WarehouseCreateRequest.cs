namespace ErpManagement.Domain.DTOs.Request.Organization.Warehouse;

public class WarehouseCreateRequest
{
    [Display(Name = Annotations.Warehouse), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = Annotations.BranchId), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int BranchId { get; set; }

    [Display(Name = Annotations.Phone)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string? Phone { get; set; }

    [Display(Name = Annotations.Email)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string? Email { get; set; }


    [Display(Name = Annotations.Code)]
    [MaxLength(20, ErrorMessage = Annotations.MaxLengthIs10)]
    public string? ZipCode { get; set; }

    [Display(Name = Annotations.Address)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? Address { get; set; }

    [Display(Name = Annotations.Description)]
    [MaxLength(1000, ErrorMessage = Annotations.MaxLengthIs1000)]
    public string? Description { get; set; }
}