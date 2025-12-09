namespace ErpManagement.Domain.DTOs.Response.Organization.Branch;

public class BranchGetAllResponse : PaginationData<BranchPaginatedData>
{
}

public class BranchPaginatedData : SelectListMoreResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? City { get; set; }
    public string? CountryName { get; set; }
    public string? Phone { get; set; }

    public bool IsActive { get; set; }
}