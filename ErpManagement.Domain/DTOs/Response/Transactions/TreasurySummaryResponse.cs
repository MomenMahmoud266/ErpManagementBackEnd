namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class TreasurySummaryResponse
{
    public int BranchId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }

    public decimal TotalOpeningCash { get; set; }
    public decimal TotalCashIn { get; set; }
    public decimal TotalCashOut { get; set; }
    public decimal TotalExpectedCash { get; set; }
    public decimal TotalClosingCash { get; set; }
    public decimal TotalDifference { get; set; }

    public List<TreasuryShiftSummaryDto> Shifts { get; set; } = new();
}

public class TreasuryShiftSummaryDto
{
    public int ShiftId { get; set; }
    public int CashboxId { get; set; }
    public string CashboxName { get; set; } = string.Empty;

    public DateTime OpenedAt { get; set; }
    public decimal OpeningCash { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedAt { get; set; }
    public decimal ExpectedCash { get; set; }
    public decimal? ClosingCash { get; set; }
    public decimal Difference { get; set; }
    public decimal CashIn { get; set; }
    public decimal CashOut { get; set; }
}
