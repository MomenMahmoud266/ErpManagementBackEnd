using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Purchases;

public class PurchaseItemCreateRequest
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

public class PurchaseCreateRequest
{
    [Display(Name = SDStatic.Annotations.Supplier)]
    [Required(ErrorMessage = SDStatic.Annotations.FieldIsRequired)]
    public int SupplierId { get; set; }

    [Display(Name = SDStatic.Annotations.Warehouse)]
    public int? WarehouseId { get; set; }

    [Display(Name = "PurchaseCode")]
    [MaxLength(50)]
    public string? PurchaseCode { get; set; }

    [Display(Name = "PurchaseDate")]
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingAmount { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public List<PurchaseItemCreateRequest> Items { get; set; } = new();
}