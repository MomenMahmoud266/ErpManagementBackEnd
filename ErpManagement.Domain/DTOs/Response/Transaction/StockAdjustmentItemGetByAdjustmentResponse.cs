namespace ErpManagement.Domain.DTOs.Response.Transaction;

public class StockAdjustmentItemGetByAdjustmentResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public string? Remark { get; set; }
}

public class StockAdjustmentGetByIdResponse
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public string AdjustmentCode { get; set; } = string.Empty;
    public DateTime AdjustmentDate { get; set; }
    public decimal TotalQuantity { get; set; }
    public string? Reason { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; }
    public string? WarehouseName { get; set; }
    public List<StockAdjustmentItemGetByAdjustmentResponse>? Items { get; set; }
}