
namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockAdjustmentGetAllFiltrationsForStockAdjustmentsRequest : PaginationRequest
{
    public int? WarehouseId { get; set; }
    public int? ProductId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? IsActive { get; set; }
}