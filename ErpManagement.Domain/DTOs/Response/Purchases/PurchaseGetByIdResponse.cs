namespace ErpManagement.Domain.DTOs.Response.Purchases;

public class PurchaseGetByIdResponse
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public string PurchaseCode { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string PurchaseStatus { get; set; } = string.Empty;
    public string? Note { get; set; }

    public List<PurchaseItemGetByPurchaseResponse> Items { get; set; } = new();
}