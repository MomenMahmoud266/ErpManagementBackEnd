// ErpManagement.Domain\DTOs\Response\Transactions\PurchaseInvoiceGetByIdResponse.cs
using System.Collections.Generic;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PurchaseInvoiceItemDto
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class PurchaseInvoiceGetByIdResponse
{
    public int Id { get; set; }
    public string PurchaseCode { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public string? SupplierName { get; set; }
    public string? WarehouseName { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string PurchaseStatus { get; set; } = string.Empty;
    public string? Note { get; set; }
    public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
}