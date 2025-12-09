namespace ErpManagement.Domain.Options;

/// <summary>
/// Tenant configuration options (not the domain entity)
/// </summary>
public class TenantConfig
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}