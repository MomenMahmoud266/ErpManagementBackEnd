namespace ErpManagement.Domain.DTOs.Response.Shared.Appointment;

public class AppointmentGetAllResponse
{
    public int TotalRecords { get; set; }
    public List<PaginatedAppointmentsData> Items { get; set; } = new();
}
