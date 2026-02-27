namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class InventoryPeriodGetByIdResponse
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedAt { get; set; }
    public decimal BeginningValue { get; set; }
    public decimal PurchasesValue { get; set; }
    public decimal EndingValue { get; set; }
    public decimal CogsValue { get; set; }
    public List<PhysicalCountDto> PhysicalCounts { get; set; } = new();
}

public class InventoryPeriodGetAllResponse
{
    public List<InventoryPeriodGetByIdResponse> Items { get; set; } = new();
    public int TotalRecords { get; set; }
}

public class PhysicalCountDto
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public decimal CountQty { get; set; }
    public decimal CostUsed { get; set; }
    public decimal LineValue { get; set; }
}
