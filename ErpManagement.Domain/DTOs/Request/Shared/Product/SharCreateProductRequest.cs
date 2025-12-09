using Net.YSolution.Sac.Recruitment.Domain.Models.Shared;

namespace ErpManagement.Domain.DTOs.Request.Shared.Country;

public class SharCreateProductRequest
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public string InternationalBarcode { get; set; } = string.Empty;
    public string notes { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int UnitId { get; set; }
    public int MinimumQuantity { get; set; }
    public double AveragePrice { get; set; }
    public double PurchasePrice { get; set; }
    public double Price { get; set; }
    public double ProfitPercentage { get; set; }
    public int? SupplierId { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string PurchaseOrder { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string SWeight { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int StockId { get; set; }

}
