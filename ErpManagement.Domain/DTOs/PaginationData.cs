namespace ErpManagement.Domain.DTOs;

public class PaginationData<T> where T : class
{
    public int TotalRecords { get; set; } = 0;
    public List<T> Items { get; set; }  = [];
}
