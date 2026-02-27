using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.Products;

public class WarehouseProduct : TenantEntity
{
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ReservedQuantity { get; set; } // optional (for orders, POS holds)

    [Column(TypeName = "decimal(18,2)")]
    public decimal AverageCost { get; set; } = 0; // Weighted Average Cost for perpetual costing

    // Navigation
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
