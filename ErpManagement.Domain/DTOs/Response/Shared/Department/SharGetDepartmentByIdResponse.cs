namespace ErpManagement.Domain.DTOs.Response.Shared.Country;

public class SharGetDepartmentByIdResponse
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


