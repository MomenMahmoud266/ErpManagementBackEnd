// ErpManagement.Domain.DTOs.Response.Transaction/PaginatedStockMovementsData.cs
using System;

namespace ErpManagement.Domain.DTOs.Response.Transaction;

public class PaginatedStockMovementsData : SelectListMoreResponse
{
    public string? ProductName { get; set; }
    public string? WarehouseName { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTime MovementDate { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public bool IsActive { get; set; }
}