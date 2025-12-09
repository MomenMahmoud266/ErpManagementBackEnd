using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockAdjustmentItemUpdateRequest : StockAdjustmentItemCreateRequest
{
    public int Id { get; set; }
}