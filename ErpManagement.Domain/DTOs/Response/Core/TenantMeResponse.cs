namespace ErpManagement.Domain.DTOs.Response.Core;

public class TenantMeResponse
{
    public int TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;

    public string BusinessType { get; set; } = string.Empty;
    public bool EnableInventory { get; set; }
    public bool EnableAppointments { get; set; }
    public bool EnableMemberships { get; set; }
    public bool EnableTables { get; set; }
    public bool EnableKitchenRouting { get; set; }

    // Subscription / Trial
    public DateTime? TrialEndsAt { get; set; }
    public DateTime? SubscriptionEndsAt { get; set; }
    public bool IsSubscriptionActive { get; set; }
    public bool IsExpired { get; set; }

    // International
    public int CurrencyId { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string CurrencySymbol { get; set; } = string.Empty;
    public int CurrencyDecimalDigits { get; set; }
    public string CountryCode { get; set; } = "EG";
    public string TimeZoneId { get; set; } = "Africa/Cairo";
    public string TaxLabel { get; set; } = "VAT";

    // Inventory costing settings
    public string InventoryMode { get; set; } = "Perpetual";
    public string CostingMethod { get; set; } = "Average";
}
