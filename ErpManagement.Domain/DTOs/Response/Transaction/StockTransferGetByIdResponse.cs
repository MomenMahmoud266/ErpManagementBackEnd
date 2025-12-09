using System;
using System.Collections.Generic;

namespace ErpManagement.Domain.DTOs.Response.Transaction;

public class StockTransferGetByIdResponse
{
    public int Id { get; set; }
    public int FromWarehouseId { get; set; }
    public int ToWarehouseId { get; set; }
    public string TransferCode { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public decimal TotalQuantity { get; set; }
    public string TransferStatus { get; set; } = string.Empty;
    public string? Remark { get; set; }
    public string? TransferNote { get; set; }
    public bool IsActive { get; set; }

    public string? FromWarehouseName { get; set; }
    public string? ToWarehouseName { get; set; }

    public List<StockTransferItemGetByTransferResponse>? Items { get; set; }
}

public class StockTransferItemGetByTransferResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public string? ProductName { get; set; }
    public string? Remark { get; set; }
}