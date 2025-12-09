namespace ErpManagement.Domain.DTOs.Response.Products;

public class ProductGetAllResponse : PaginationData<ProductPaginatedData>
{
}

public class ProductPaginatedData : SelectListMoreResponse
{
    public string? ProductCode { get; set; }
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
}