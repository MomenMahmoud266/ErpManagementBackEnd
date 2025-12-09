using ErpManagement.Domain.DTOs.Request;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class SaleGetAllFiltrationsForSalesRequest : PaginationRequest
{
    // Add filtering fields if required later (date range, customer, warehouse, status)
    public int? CustomerId { get; set; }
    public int? WarehouseId { get; set; }
    public string? SaleStatus { get; set; }
}