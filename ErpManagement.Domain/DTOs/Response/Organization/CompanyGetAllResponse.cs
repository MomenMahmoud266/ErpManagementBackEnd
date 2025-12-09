namespace ErpManagement.Domain.DTOs.Response.Organization.Company;

public class CompanyGetAllResponse : PaginationData<CompanyPaginatedData>
{
}

public class CompanyPaginatedData : SelectListMoreResponse
{
    public bool IsActive { get; set; }
}