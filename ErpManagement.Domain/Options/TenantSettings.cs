namespace ErpManagement.Domain.Options;

public class TenantSettings
{
    public TenancyConfig Defaults { get; set; } = default!;
    public List<TenantConfig> Tenants { get; set; } = [];
}