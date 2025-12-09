namespace ErpManagement.Domain.Models.Shared;

[Table("Shar_Department")]
public class SharDepartment : FullStaticDataEntity
{
    public int MainDepartment { get; set; }
    public int ArrangeNumber { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public bool showLowQuantities { get; set; }
    public bool showInSales { get; set; }
    
    
   
}
