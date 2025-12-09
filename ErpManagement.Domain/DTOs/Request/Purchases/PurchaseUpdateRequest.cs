using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Purchases;

public class PurchaseUpdateRequest : PurchaseCreateRequest
{
    [Required(ErrorMessage = SDStatic.Annotations.FieldIsRequired)]
    public int Id { get; set; }
}