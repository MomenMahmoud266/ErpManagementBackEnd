using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class ShiftLedgerResponse
{
    public int ShiftId { get; set; }
    public int CashboxId { get; set; }
    public string CashboxName { get; set; } = string.Empty;

    public DateTime OpenedAt { get; set; }
    public decimal OpeningCash { get; set; }

    public bool IsClosed { get; set; }
    public DateTime? ClosedAt { get; set; }
    public decimal? ClosingCash { get; set; }

    public decimal TotalCashIn { get; set; }
    public decimal TotalCashOut { get; set; }

    public decimal ExpectedCash { get; set; }   // Opening + In - Out
    public decimal Difference { get; set; }     // Closing - Expected (only meaningful when closed)

    public List<ShiftLedgerLineDto> Lines { get; set; } = new();
}

public class ShiftLedgerLineDto
{
    public DateTime Date { get; set; }
    public int MovementId { get; set; }
    public CashMovementType Type { get; set; }
    public string TypeLabel => Type == CashMovementType.CashIn ? "CashIn" : "CashOut";
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public decimal Balance { get; set; } // running balance, starts at OpeningCash
}
