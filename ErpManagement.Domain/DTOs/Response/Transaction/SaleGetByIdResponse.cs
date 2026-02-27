namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class SaleGetByIdResponse
{
    public int Id { get; set; }
    public string SaleCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public int? BillerId { get; set; }
    public string? BillerName { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string SaleStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? Note { get; set; }
    public bool IsActive { get; set; }
    public decimal PaidAmount { get; set; }
    public string? LastPaymentType { get; set; }
    public decimal ChangeAmount { get; set; }

    public ICollection<SaleItemGetBySaleResponse> Items { get; set; } = new List<SaleItemGetBySaleResponse>();
}
