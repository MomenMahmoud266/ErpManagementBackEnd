using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Request.Shared.Appointment;

public class AppointmentGetAllFiltrationsRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public int? BranchId { get; set; }
    public string? StaffUserId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public AppointmentStatus? Status { get; set; }
}
