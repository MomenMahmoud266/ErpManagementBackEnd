using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class ExpenseCategoryUpdateRequest
{
    [Required]
    public int Id { get; set; }

    [Display(Name = Annotations.NameAr)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(150)]
    public string NameAr { get; set; } = string.Empty;

    [Display(Name = Annotations.NameEn)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(150)]
    public string NameEn { get; set; } = string.Empty;

    [Display(Name = Annotations.NameTr)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(150)]
    public string NameTr { get; set; } = string.Empty;
}