namespace ErpManagement.Domain.DTOs.Request.Shared.Country;

public class SharCreateBranchRequest
{
    public  string NameAr { get; set; } = string.Empty;
    public  string NameEn { get; set; } = string.Empty;
    public List<stockObj> ListOfStocks { get; set; } = [];
}
public class stockObj
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;

}