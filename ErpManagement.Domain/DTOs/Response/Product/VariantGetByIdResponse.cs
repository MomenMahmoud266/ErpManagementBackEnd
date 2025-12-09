namespace ErpManagement.Domain.DTOs.Response.Shared.Variant;

public class VariantGetByIdResponse
{
    public int Id { get; set; }

    public required string VariantType { get; set; }
    public required string Name { get; set; }
    public string? Code { get; set; }
}