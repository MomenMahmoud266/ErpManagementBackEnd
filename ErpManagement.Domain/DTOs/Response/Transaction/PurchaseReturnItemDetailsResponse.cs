// ErpManagement.Domain\DTOs\Response\Transactions\PurchaseReturnResponses.cs
using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class PurchaseReturnItemDetailsResponse
{
    [Display(Name = SDStatic.Annotations.Product)]
    public int ProductId { get; set; }

    [Display(Name = SDStatic.Annotations.Product)]
    public string? ProductName { get; set; }

    [Display(Name = SDStatic.Annotations.Quantity)]
    public decimal Quantity { get; set; }

    [Display(Name = SDStatic.Annotations.UnitType)]
    public decimal UnitPrice { get; set; }

    [Display(Name = SDStatic.Annotations.Amount)]
    public decimal Amount { get; set; }

    [Display(Name = SDStatic.Annotations.Tax)]
    public decimal TaxAmount { get; set; }

    [Display(Name = SDStatic.Annotations.Discount)]
    public decimal Discount { get; set; }

    public decimal TotalAmount { get; set; }
}

public class PurchaseReturnGetByIdResponse
{
    public int Id { get; set; }
    public int? PurchaseId { get; set; }
    public int SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public string? ReturnCode { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime ReturnDate { get; set; }
    public string? ReturnStatus { get; set; }
    public string? Remark { get; set; }
    public string? ReturnNote { get; set; }
    public bool IsActive { get; set; }

    public string? SupplierName { get; set; }
    public string? WarehouseName { get; set; }
    public string? PurchaseCode { get; set; }

    public ICollection<PurchaseReturnItemDetailsResponse>? Items { get; set; }
}

public class PaginatedPurchaseReturnsData : SelectListMoreResponse
{
    public int Id { get; set; }
    public string? ReturnCode { get; set; }
    public string? PurchaseCode { get; set; }
    public string? SupplierName { get; set; }
    public string? WarehouseName { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? ReturnStatus { get; set; }
    public bool IsActive { get; set; }
}

public class PurchaseReturnGetAllResponse : PaginationData<PaginatedPurchaseReturnsData>
{
}