namespace ErpManagement.Domain.DTOs.Response.Transactions;


public class ExpenseCategoryGetAllResponse : PaginationData<PaginatedExpenseCategoriesData>
{
}

public class PaginatedExpenseCategoriesData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}