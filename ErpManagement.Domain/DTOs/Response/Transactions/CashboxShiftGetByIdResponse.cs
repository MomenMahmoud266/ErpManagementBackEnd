using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class CashboxShiftGetByIdResponse
{
    public int Id { get; set; }
    public int CashboxId { get; set; }
    public string CashboxName { get; set; } = string.Empty;
    public string OpenedByUserId { get; set; } = string.Empty;
    public DateTime OpenedAt { get; set; }
    public decimal OpeningCash { get; set; }
    public DateTime? ClosedAt { get; set; }
    public decimal? ClosingCash { get; set; }
    public decimal ExpectedCash { get; set; }
    public decimal Difference { get; set; }
    public bool IsClosed { get; set; }
    public List<CashMovementDto> Movements { get; set; } = new();
}

public class CashMovementDto
{
    public int Id { get; set; }
    public CashMovementType Type { get; set; }
    public string TypeLabel => Type == CashMovementType.CashIn ? "CashIn" : "CashOut";
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
}
