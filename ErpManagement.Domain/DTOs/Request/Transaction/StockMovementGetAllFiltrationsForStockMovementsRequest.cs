using System;

namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockMovementGetAllFiltrationsForStockMovementsRequest : PaginationRequest
{
    public int? ProductId { get; set; }
    public int? WarehouseId { get; set; }
    public string? MovementType { get; set; }    // e.g., "Sale", "Purchase"
    public string? Direction { get; set; }       // "In" or "Out"
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? ReferenceType { get; set; }   // "Sale", "Purchase", etc
    public int? ReferenceId { get; set; }
    public bool? IsActive { get; set; }
}