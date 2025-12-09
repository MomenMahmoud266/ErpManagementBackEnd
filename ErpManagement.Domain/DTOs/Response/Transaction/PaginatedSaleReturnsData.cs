using ErpManagement.Domain.DTOs.Response.Shared;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PaginatedSaleReturnsData : SelectListMoreResponse
{
    public string? ReturnCode { get; set; }
    public string? SaleCode { get; set; }
    public string? CustomerName { get; set; }
    public string? WarehouseName { get; set; }
    public string? BillerName { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? ReturnStatus { get; set; }
    public bool IsActive { get; set; }
}