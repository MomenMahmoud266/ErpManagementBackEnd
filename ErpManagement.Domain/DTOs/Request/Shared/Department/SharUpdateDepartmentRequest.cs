namespace ErpManagement.Domain.DTOs.Request.Shared.Country;

public class SharUpdateDepartmentRequest
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public int MainDepartment { get; set; }
    public int ArrangeNumber { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public bool showLowQuantities { get; set; }
    public bool showInSales { get; set; }
}
