using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Auth;

namespace ErpManagement.Domain.Models.Transactions;

public class StockTransfer : TenantEntity
{
    public int FromWarehouseId { get; set; }
    public int ToWarehouseId { get; set; }

    [MaxLength(50)]
    public string TransferCode { get; set; } = string.Empty;

    public DateTime TransferDate { get; set; }

    public decimal TotalQuantity { get; set; }

    [MaxLength(50)]
    public string TransferStatus { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Remark { get; set; }

    [MaxLength(1000)]
    public string? TransferNote { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public Warehouse FromWarehouse { get; set; } = null!;
    public Warehouse ToWarehouse { get; set; } = null!;
    public ApplicationUser? User { get; set; }

    public ICollection<StockTransferItem>? Items { get; set; }
}