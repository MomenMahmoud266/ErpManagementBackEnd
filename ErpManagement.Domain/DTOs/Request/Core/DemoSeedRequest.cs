namespace ErpManagement.Domain.DTOs.Request.Core;

/// <summary>
/// Configuration for the POST /api/demo/seed endpoint.
/// All fields are optional; the listed defaults are used when not supplied.
/// </summary>
public class DemoSeedRequest
{
    public int BranchCount { get; set; } = 2;
    public int WarehousesPerBranch { get; set; } = 1;
    public int ProductCount { get; set; } = 200;
    public int CustomerCount { get; set; } = 100;
    public int DaysOfSales { get; set; } = 30;
    public int SalesPerDay { get; set; } = 50;
}
