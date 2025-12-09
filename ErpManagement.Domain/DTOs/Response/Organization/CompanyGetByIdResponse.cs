namespace ErpManagement.Domain.DTOs.Response.Organization.Company;

public class CompanyGetByIdResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}