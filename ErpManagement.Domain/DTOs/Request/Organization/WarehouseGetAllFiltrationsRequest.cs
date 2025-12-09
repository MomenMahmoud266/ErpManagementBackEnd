namespace ErpManagement.Domain.DTOs.Request.Organization.Warehouse;

public class WarehouseGetAllFiltrationsRequest : PaginationRequest
{
    public int? BranchId { get; set; }
}