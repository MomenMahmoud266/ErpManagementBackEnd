namespace ErpManagement.Domain.DTOs.Request.Shared.Country;

public class SharUpdateSupplierRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public int AccountNumber { get; set; }
    public int PurchaseAccount { get; set; }
    public string Fax { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Page { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public int TaxFileNumber { get; set; }
    public int TaxCardNumber { get; set; }
    public int TaxSalesNumber { get; set; }
    public int CommercialRegistrationNo { get; set; }
}
