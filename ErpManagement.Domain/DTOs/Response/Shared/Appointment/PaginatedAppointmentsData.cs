using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Response.Shared.Appointment;

public class PaginatedAppointmentsData
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string StaffUserId { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public AppointmentStatus Status { get; set; }
    public int? SaleId { get; set; }
}
