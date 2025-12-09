using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class SaleUpdateRequest
{
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int Id { get; set; }

    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int CustomerId { get; set; }

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