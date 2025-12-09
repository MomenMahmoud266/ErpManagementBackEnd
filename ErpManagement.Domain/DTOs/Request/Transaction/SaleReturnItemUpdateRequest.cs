using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class SaleReturnItemUpdateRequest
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public decimal Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    public decimal? TaxAmount { get; set; }
    public decimal? Discount { get; set; }
}