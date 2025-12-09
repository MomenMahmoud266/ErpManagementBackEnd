namespace ErpManagement.Domain.DTOs.Response.Shared;

public class ProductTypeGetByIdResponse
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public required string Title { get; set; }
    public string Type { get; set; } = "Product";
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}