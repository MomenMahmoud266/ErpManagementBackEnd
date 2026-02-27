namespace ErpManagement.Domain.DTOs.Response.Transactions;

public class ProfitLossResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }

    // Revenue
    public decimal Revenue { get; set; }

    // Cost of Goods Sold
    public decimal Cogs { get; set; }
    public decimal GrossProfit { get; set; }

    // Operating expenses
    public decimal Expenses { get; set; }

    // Bottom line
    public decimal NetProfit { get; set; }

    // Optional breakdowns
    public List<DailyRevenueDto> DailyRevenue { get; set; } = new();
    public List<ExpenseByCategoryDto> ExpensesByCategory { get; set; } = new();

    // Meta
    public string InventoryMode { get; set; } = string.Empty;
    /// <summary>"Perpetual-Average", "Periodic", "NotAvailable"</summary>
    public string CogsSource { get; set; } = string.Empty;
}

public class DailyRevenueDto
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
}

public class ExpenseByCategoryDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
