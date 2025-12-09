using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class SaleCreateRequest
{
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int CustomerId { get; set; }

    [Display(Name = Annotations.Warehouse)]
    public int? WarehouseId { get; set; }

    public int? BillerId { get; set; }

    [MaxLength(50)]
    public string? SaleCode { get; set; }

    public DateTime SaleDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingAmount { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public ICollection<SaleItemCreateRequest> Items { get; set; } = new List<SaleItemCreateRequest>();
}