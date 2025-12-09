namespace ErpManagement.Domain.DTOs.Response.Organization.Warehouse;

public class WarehouseGetByIdResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ZipCode { get; set; }
    public string? Address { get; set; }
    public string? warehouseCode { get; set; }
    public int BranchId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}