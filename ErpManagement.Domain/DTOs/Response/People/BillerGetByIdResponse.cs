namespace ErpManagement.Domain.DTOs.Response.People.Biller;

public class BillerGetByIdResponse
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int CountryId { get; set; }
    public required string BillerCode { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public string? NidPassportNumber { get; set; }
    public DateTime? DateOfJoin { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }
}