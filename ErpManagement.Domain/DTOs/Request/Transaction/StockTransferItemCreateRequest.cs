using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockTransferItemCreateRequest
{
    [Display(Name = Annotations.Product)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int ProductId { get; set; }

    [Display(Name = Annotations.Quantity)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public decimal Quantity { get; set; }

    [MaxLength(500)]
    public string? Remark { get; set; }
}