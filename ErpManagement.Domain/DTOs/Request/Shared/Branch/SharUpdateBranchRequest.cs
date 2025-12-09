namespace ErpManagement.Domain.DTOs.Request.Shared.Country;

public class SharUpdateBranchRequest
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public int SalesAccount { get; set; }
    public int BoxAccount { get; set; }
    public int BackAccount { get; set; }
    public int KeyNetAccount { get; set; }
    public int PurchaseAccount { get; set; }
    public int SuppliersAccount { get; set; }
    public int Chekat { get; set; }
    public int ReturnedPurchase { get; set; }
    public int ReturnedSales { get; set; }
    public int PurchasingExpenses { get; set; }
    public int ExpensesAccount { get; set; }
    public int ClientsAccount { get; set; }
    public int SalesWithAccount { get; set; }
    public int SalesDiscountWithSub { get; set; }
    public int Master { get; set; }
    public int CenterPriceNumber { get; set; }
    public List<stockObj> ListOfStocks { get; set; } = [];
}
