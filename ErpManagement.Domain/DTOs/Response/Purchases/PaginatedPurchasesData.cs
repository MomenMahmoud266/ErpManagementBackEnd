namespace ErpManagement.Domain.DTOs.Response.Purchases;

public class PaginatedPurchasesData
{
    public int Id { get; set; }
    public string PurchaseCode { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public string? WarehouseName { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string PurchaseStatus { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}