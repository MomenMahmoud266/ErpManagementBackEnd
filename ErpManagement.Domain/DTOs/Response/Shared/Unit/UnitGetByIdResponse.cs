namespace ErpManagement.Domain.DTOs.Response.Shared.Unit;

public class UnitGetByIdResponse 
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string UnitType { get; set; }
    public string? Symbol { get; set; }
}