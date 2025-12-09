namespace ErpManagement.Domain.DTOs.Response.People.Customer;

public class CustomerGetByIdResponse
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public int? CategoryId { get; set; }
    public required string CustomerCode { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public decimal RewardPoints { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }
}