namespace ErpManagement.Domain.Models;

public abstract class StaticDataEntity 
{
    [Key]
    public int Id { get; set; }
    public required string NameAr { get; set; }
    public required string NameEn { get; set; }
    public required string NameTr { get; set; }
}


