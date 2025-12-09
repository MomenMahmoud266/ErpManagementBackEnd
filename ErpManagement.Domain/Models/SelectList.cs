namespace ErpManagement.Domain.Models;

public abstract class SelectList : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string NameInEnglish { get; set; }
}
public abstract class SpecificSelectList : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
}


