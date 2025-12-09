namespace ErpManagement.Domain.DTOs.Response.Shared.Category;

public class CategoryGetByIdResponse 
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public required string Title { get; set; }
    public required string Type { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}