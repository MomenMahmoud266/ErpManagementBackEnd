namespace ErpManagement.Domain.DTOs.Request.Products;

public class ProductUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = Annotations.ProductCode), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(50, ErrorMessage = Annotations.MaxLengthIs50)]
    public string ProductCode { get; set; } = string.Empty;

    [Display(Name = Annotations.Title), Required(ErrorMessage = Annotations.FieldIsRequired)]
    [MaxLength(200, ErrorMessage = Annotations.MaxLengthIs200)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = Annotations.Description)]
    [MaxLength(1000, ErrorMessage = Annotations.MaxLengthIs1000)]
    public string? Description { get; set; }

    [Display(Name = Annotations.Category), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int CategoryId { get; set; }

    [Display(Name = Annotations.Supplier)]
    public int? SupplierId { get; set; }

    [Display(Name = Annotations.Brand)]
    public int? BrandId { get; set; }

    [Display(Name = Annotations.Type)]
    public int? TypeId { get; set; }

    [Display(Name = Annotations.Unit)]
    public int? UnitId { get; set; }

    [Display(Name = Annotations.Tax)]
    public int? TaxId { get; set; }

    [Display(Name = Annotations.Price), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public decimal Price { get; set; }

    [Display(Name = Annotations.Cost)]
    public decimal? Cost { get; set; }

    [Display(Name = Annotations.Tax)]
    public decimal? Tax { get; set; }

    [Display(Name = Annotations.Discount)]
    public decimal? Discount { get; set; }

    [Display(Name = Annotations.Quantity), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public decimal Quantity { get; set; }

    [Display(Name = Annotations.AlertQuantity)]
    public decimal? AlertQuantity { get; set; }

    [Display(Name = Annotations.ImagePath)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? ImagePath { get; set; }

    [Display(Name = Annotations.Barcode)]
    [MaxLength(100, ErrorMessage = Annotations.MaxLengthIs100)]
    public string? Barcode { get; set; }

    [Display(Name = Annotations.SKU)]
    [MaxLength(100, ErrorMessage = Annotations.MaxLengthIs100)]
    public string? SKU { get; set; }

    [Display(Name = Annotations.IsFeatured)]
    public bool IsFeatured { get; set; }

    [Display(Name = Annotations.IsExpired)]
    public bool IsExpired { get; set; }

    [Display(Name = Annotations.IsPromoSale)]
    public bool IsPromoSale { get; set; }

    [Display(Name = Annotations.ExpiryDate)]
    public DateTime? ExpiryDate { get; set; }

    [Display(Name = Annotations.ManufactureDate)]
    public DateTime? ManufactureDate { get; set; }

    [Display(Name = Annotations.ColorVariantIds)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? ColorVariantIds { get; set; }

    [Display(Name = Annotations.SizeVariantIds)]
    [MaxLength(500, ErrorMessage = Annotations.MaxLengthIs500)]
    public string? SizeVariantIds { get; set; }
}