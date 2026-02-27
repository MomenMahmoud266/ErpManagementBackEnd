using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Request.Shared.Appointment;

public class AppointmentItemCreateRequest
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
}
