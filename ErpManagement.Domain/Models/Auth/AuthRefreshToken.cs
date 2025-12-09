namespace ErpManagement.Domain.Models.Auth;

[Table("Auth_RefreshTokens")]
public class AuthRefreshToken : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; }
    public bool IsExpired => new DateTime().NowEg() >= ExpiresOn;
    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
    public bool IsActivated => RevokedOn == null && !IsExpired;

    public string User_Id { get; set; } = string.Empty;
    [ForeignKey(nameof(User_Id))]
    public ApplicationUser User { get; set; } = default!;
}
