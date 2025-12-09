namespace ErpManagement.Domain.DTOs.Response.Shared.Tax;

public class TaxGetByIdResponse 
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}