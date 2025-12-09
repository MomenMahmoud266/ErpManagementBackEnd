namespace ErpManagement.Domain.Models.Auth;

public class ApplicationUserRole : IdentityUserRole<string>, IBaseEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? InsertDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }

    public string? InsertBy { get; set; }
    public string? UpdateBy { get; set; }
    public string? DeleteBy { get; set; }
    public bool IsActive { get; set; } = true;
}
