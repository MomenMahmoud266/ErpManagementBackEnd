using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class SaleReturnUpdateRequest
{
    public int Id { get; set; }

    [Required]
    public int SaleId { get; set; }

    [Required]
    public int CustomerId { get; set; }

    public int? WarehouseId { get; set; }
    public int? BillerId { get; set; }

    public string? ReturnCode { get; set; }

    [Required]
    public DateTime ReturnDate { get; set; }

    public decimal ShippingAmount { get; set; }

    public string? Remark { get; set; }
    public string? ReturnNote { get; set; }

    [Required]
    public List<SaleReturnItemUpdateRequest> Items { get; set; } = new();
}