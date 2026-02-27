using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Clinic;

public class AppointmentItem : BaseEntity
{
    public int AppointmentId { get; set; }
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
