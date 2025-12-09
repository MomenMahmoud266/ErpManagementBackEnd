using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockAdjustmentItemCreateRequest
{
    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    [MaxLength(500)]
    public string? Remark { get; set; }
}