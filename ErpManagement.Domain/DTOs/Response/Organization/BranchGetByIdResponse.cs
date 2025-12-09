namespace ErpManagement.Domain.DTOs.Response.Organization.Branch;

public class BranchGetByIdResponse
{
    public int Id { get; set; }

    public string NameEn { get; set; } = string.Empty;

    public string? NameAr { get; set; }

    public string? NameTr { get; set; }

    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public bool IsActive { get; set; }
}