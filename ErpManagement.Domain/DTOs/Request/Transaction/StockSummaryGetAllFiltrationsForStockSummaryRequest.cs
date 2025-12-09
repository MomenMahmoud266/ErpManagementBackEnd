// ErpManagement.Domain.DTOs.Request.Transaction/StockSummaryGetAllFiltrationsForStockSummaryRequest.cs
namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockSummaryGetAllFiltrationsForStockSummaryRequest : PaginationRequest
{
    public int? ProductId { get; set; }
    public int? WarehouseId { get; set; }
    public decimal? MinQuantity { get; set; }
    public decimal? MaxQuantity { get; set; }
    public bool? IsActive { get; set; }
}