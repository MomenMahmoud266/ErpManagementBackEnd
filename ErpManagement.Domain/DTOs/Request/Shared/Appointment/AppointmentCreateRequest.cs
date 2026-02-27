namespace ErpManagement.Domain.DTOs.Request.Shared.Appointment;

public class AppointmentCreateRequest
{
    public int BranchId { get; set; }
    public int CustomerId { get; set; }
    public string StaffUserId { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string? Notes { get; set; }
    public List<AppointmentItemCreateRequest> Items { get; set; } = new();
}
