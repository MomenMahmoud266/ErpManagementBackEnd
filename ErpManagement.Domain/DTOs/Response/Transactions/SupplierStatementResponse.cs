namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class SupplierStatementResponse
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal TotalPayments { get; set; }
    public decimal TotalReturns { get; set; }
    public decimal ClosingBalance { get; set; }
    public List<SupplierStatementLineDto> Lines { get; set; } = new();
}

public class SupplierStatementLineDto
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
}
