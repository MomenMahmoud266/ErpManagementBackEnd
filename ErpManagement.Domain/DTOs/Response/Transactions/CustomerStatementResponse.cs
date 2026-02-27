namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class CustomerStatementResponse
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalPayments { get; set; }
    public decimal TotalReturns { get; set; }
    public decimal ClosingBalance { get; set; }
    public List<CustomerStatementLineDto> Lines { get; set; } = new();
}

public class CustomerStatementLineDto
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
}
