// ErpManagement.Domain\DTOs\Response\Transactions\SaleInvoiceGetByIdResponse.cs
using System.Collections.Generic;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class SaleInvoiceItemDto
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

public class SaleInvoiceGetByIdResponse
{
    public int Id { get; set; }
    public string SaleCode { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? CustomerAddress { get; set; }
    public string? WarehouseName { get; set; }
    public string? BillerName { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string SaleStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? Note { get; set; }
    public List<SaleInvoiceItemDto> Items { get; set; } = new();
}