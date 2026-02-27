using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.Domain.Models.Clinic;

public class Appointment : TenantEntity
{
    public int BranchId { get; set; }
    public int CustomerId { get; set; }
    public string StaffUserId { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string? Notes { get; set; }
    public int? SaleId { get; set; }

    public virtual Branch Branch { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
    public virtual Sale? Sale { get; set; }
    public virtual ICollection<AppointmentItem> Items { get; set; } = new HashSet<AppointmentItem>();
}
