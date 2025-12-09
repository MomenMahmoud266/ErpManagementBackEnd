using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class SaleReturnGetByIdResponse
{
    public int Id { get; set; }

    public int SaleId { get; set; }
    public string? SaleCode { get; set; }

    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }

    public int? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }

    public int? BillerId { get; set; }
    public string? BillerName { get; set; }

    public string? ReturnCode { get; set; }
    public DateTime ReturnDate { get; set; }

    public decimal ShippingAmount { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }

    public string? ReturnStatus { get; set; }
    public string? Remark { get; set; }
    public string? ReturnNote { get; set; }

    public bool IsActive { get; set; }

    public IEnumerable<SaleReturnItemDetailsResponse> Items { get; set; } = Array.Empty<SaleReturnItemDetailsResponse>();
}