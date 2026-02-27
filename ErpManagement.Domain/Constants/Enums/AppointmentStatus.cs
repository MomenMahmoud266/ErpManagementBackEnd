namespace ErpManagement.Domain.Constants.Enums;

public enum AppointmentStatus : byte
{
    Scheduled = 1,
    CheckedIn = 2,
    Completed = 3,
    Cancelled = 4,
    NoShow = 5
}
