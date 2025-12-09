namespace ErpManagement.Domain.DTOs.Response.Shared.Brand;

public class BrandGetByIdResponse 
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}