namespace ErpManagement.Domain.DTOs.Response.Products;

public class ProductGetByIdResponse
{
    public int Id { get; set; }

    public required string ProductCode { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public int CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public int? UnitId { get; set; }
    public int? TaxId { get; set; }

    public decimal Price { get; set; }
    public decimal? Cost { get; set; }
    public decimal? Tax { get; set; }
    public decimal? Discount { get; set; }
    public decimal Quantity { get; set; }
    public decimal? AlertQuantity { get; set; }

    public string? ImagePath { get; set; }
    public string? Barcode { get; set; }
    public string? SKU { get; set; }

    public bool IsFeatured { get; set; }
    public bool IsExpired { get; set; }
    public bool IsPromoSale { get; set; }

    public DateTime? ExpiryDate { get; set; }
    public DateTime? ManufactureDate { get; set; }

    public bool IsActive { get; set; }

    public string? ColorVariantIds { get; set; }
    public string? SizeVariantIds { get; set; }
}