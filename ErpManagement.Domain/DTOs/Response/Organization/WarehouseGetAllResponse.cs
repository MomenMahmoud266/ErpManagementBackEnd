namespace ErpManagement.Domain.DTOs.Response.Organization.Warehouse;

public class WarehouseGetAllResponse : PaginationData<WarehousePaginatedData>
{
}

public class WarehousePaginatedData : SelectListMoreResponse
{
    public bool IsActive { get; set; }
    public string BranchName { get; set; }
    public int? BranchId { get; set; }
}