namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PaginatedSalesData : SelectListMoreResponse
{
    public string SaleCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? WarehouseName { get; set; }
    public string? BillerName { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string SaleStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}