using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class PurchaseReturnItemCreateRequest
{
    [Display(Name = SDStatic.Annotations.Product)]
    [Required(ErrorMessage = SDStatic.Annotations.FieldIsRequired)]
    public int ProductId { get; set; }

    [Display(Name = SDStatic.Annotations.Quantity)]
    [Required(ErrorMessage = SDStatic.Annotations.FieldIsRequired)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Display(Name = SDStatic.Annotations.UnitType)]
    [Required(ErrorMessage = SDStatic.Annotations.FieldIsRequired)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Discount { get; set; }
}

public class PurchaseReturnCreateRequest
{
    [Display(Name = SDStatic.Annotations.Purchase)]
    [Required(ErrorMessage = SDStatic.Annotations.FieldIsRequired)]
    public int PurchaseId { get; set; }

    [Display(Name = SDStatic.Annotations.Supplier)]
    [Required(ErrorMessage = SDStatic.Annotations.FieldIsRequired)]
    public int SupplierId { get; set; }

    [Display(Name = SDStatic.Annotations.Warehouse)]
    public int? WarehouseId { get; set; }

    [Display(Name = "ReturnCode")]
    [MaxLength(50)]
    public string? ReturnCode { get; set; }

    [Display(Name = "ReturnDate")]
    public DateTime ReturnDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingAmount { get; set; }

    [MaxLength(500)]
    public string? Remark { get; set; }

    [MaxLength(1000)]
    public string? ReturnNote { get; set; }

    public List<PurchaseReturnItemCreateRequest> Items { get; set; } = new();
}