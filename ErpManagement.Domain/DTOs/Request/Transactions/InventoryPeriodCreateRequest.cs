using System.ComponentModel.DataAnnotations;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class InventoryPeriodCreateRequest
{
    [Required]
    public int BranchId { get; set; }

    [Required]
    public DateTime From { get; set; }

    [Required]
    public DateTime To { get; set; }
}

public class PhysicalCountCreateRequest
{
    [Required]
    public int WarehouseId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public decimal CountQty { get; set; }

    [Required]
    public decimal CostUsed { get; set; }
}
