namespace ErpManagement.Domain.DTOs.Request.Shared.Appointment;

public class AppointmentCompleteInvoiceRequest
{
    public int WarehouseId { get; set; }
    public int BillerId { get; set; }
    public decimal PaidAmount { get; set; } = 0;
    public string PaymentType { get; set; } = "Cash";
    public string? TransactionNumber { get; set; }
    public string? AccountNumber { get; set; }
}
