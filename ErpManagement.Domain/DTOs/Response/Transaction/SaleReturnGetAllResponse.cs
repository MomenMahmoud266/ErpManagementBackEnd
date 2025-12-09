namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class SaleReturnGetAllResponse
{
    public IEnumerable<PaginatedSaleReturnsData> Items { get; set; } = Array.Empty<PaginatedSaleReturnsData>();
    public int TotalRecords { get; set; }
}