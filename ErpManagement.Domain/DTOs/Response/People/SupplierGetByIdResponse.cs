namespace ErpManagement.Domain.DTOs.Response.People.Supplier;

public class SupplierGetByIdResponse
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public int? CompanyId { get; set; }
    public required string SupplierCode { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }
}