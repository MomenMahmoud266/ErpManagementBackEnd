namespace ErpManagement.Domain.Options;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public TimeSpan TokenLifetime { get; set; }
}
