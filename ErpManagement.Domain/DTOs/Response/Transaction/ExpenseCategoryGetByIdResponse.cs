namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class ExpenseCategoryGetByIdResponse
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}