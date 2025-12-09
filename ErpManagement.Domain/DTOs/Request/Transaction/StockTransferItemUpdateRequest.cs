using System.ComponentModel.DataAnnotations;
using ErpManagement.Domain.Constants.Statics;

namespace ErpManagement.Domain.DTOs.Request.Transaction;

public class StockTransferItemUpdateRequest : StockTransferItemCreateRequest
{
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int Id { get; set; }
}