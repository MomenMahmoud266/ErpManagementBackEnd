namespace ErpManagement.Domain.DTOs.Response.Transaction;

public class PaginatedStockAdjustmentsData
{
    public int Id { get; set; }
    public string AdjustmentCode { get; set; } = string.Empty;
    public string? WarehouseName { get; set; }
    public DateTime AdjustmentDate { get; set; }
    public decimal TotalQuantity { get; set; }
    public bool IsActive { get; set; }
}