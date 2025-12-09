using ErpManagement.Domain.Dtos.Request;

namespace ErpManagement.Domain.DTOs.Request.Purchases;

public class PurchaseGetAllFiltrationsForPurchasesRequest : PaginationRequest
{
    public int? SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? Status { get; set; }
}