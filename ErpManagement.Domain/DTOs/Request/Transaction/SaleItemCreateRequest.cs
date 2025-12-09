using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class SaleItemCreateRequest
{
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int ProductId { get; set; }

    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Discount { get; set; }
}