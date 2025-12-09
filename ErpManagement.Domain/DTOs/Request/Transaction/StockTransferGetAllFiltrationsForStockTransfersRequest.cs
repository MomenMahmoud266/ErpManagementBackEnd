using System;
using ErpManagement.Domain.DTOs.Request;

namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockTransferGetAllFiltrationsForStockTransfersRequest : PaginationRequest
{
    public int? FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
    public int? ProductId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? IsActive { get; set; }
}