
namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockAdjustmentCreateRequest
{
    public int Id { get; set; }

    [Display(Name = Annotations.Warehouse)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int WarehouseId { get; set; }

    [MaxLength(50)]
    public string? AdjustmentCode { get; set; }

    [Display(Name = Annotations.AdjustmentDate)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public DateTime AdjustmentDate { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    [Display(Name = Annotations.Items)]
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public List<StockAdjustmentItemCreateRequest> Items { get; set; } = new();
}