namespace ErpManagement.Domain.DTOs.Response.Shared.Category;

public class CategoryGetAllResponse : PaginationData<CategoryPaginatedData>
{
}

public class CategoryPaginatedData : SelectListMoreResponse
{
    public int ParentId { get; set; }
}