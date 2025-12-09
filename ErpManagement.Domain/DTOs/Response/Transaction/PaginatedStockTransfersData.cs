using System;

namespace ErpManagement.Domain.DTOs.Response.Transaction;

public class PaginatedStockTransfersData : SelectListMoreResponse
{
    public int Id { get; set; }
    public string TransferCode { get; set; } = string.Empty;
    public string? FromWarehouseName { get; set; }
    public string? ToWarehouseName { get; set; }
    public DateTime TransferDate { get; set; }
    public decimal TotalQuantity { get; set; }
    public string TransferStatus { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}