using ErpManagement.Domain.DTOs;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class CashboxGetAllResponse : PaginationData<PaginatedCashboxesData>
{
}

public class PaginatedCashboxesData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool HasOpenShift { get; set; }
}
