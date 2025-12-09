using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Models.Shared;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Models.People;

namespace ErpManagement.Domain.Models.Core;

public class Country : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string NameEn { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? NameAr { get; set; }

    [MaxLength(100)]
    public string? NameTr { get; set; }

    [MaxLength(10)]
    public string? Code { get; set; }

    [MaxLength(10)]
    public string? PhoneCode { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<State> States { get; set; } = new HashSet<State>();
    public virtual ICollection<Warehouse> Warehouses { get; set; } = new HashSet<Warehouse>();
    public virtual ICollection<Supplier> Suppliers { get; set; } = new HashSet<Supplier>();
    public virtual ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
    public virtual ICollection<Biller> Billers { get; set; } = new HashSet<Biller>();
}