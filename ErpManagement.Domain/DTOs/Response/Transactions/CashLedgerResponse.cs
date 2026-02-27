using ErpManagement.Domain.Constants.Enums;

namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class CashLedgerResponse
{
    public int BranchId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal TotalCashIn { get; set; }
    public decimal TotalCashOut { get; set; }
    public decimal ClosingBalance { get; set; }
    public List<CashLedgerLineDto> Lines { get; set; } = new();
}

public class CashLedgerLineDto
{
    public DateTime Date { get; set; }
    public int ShiftId { get; set; }
    public CashMovementType Type { get; set; }
    public string TypeLabel => Type == CashMovementType.CashIn ? "CashIn" : "CashOut";
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public decimal Balance { get; set; }
}
