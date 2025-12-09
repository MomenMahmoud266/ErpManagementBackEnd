
namespace ErpManagement.Domain.DTOs.Request.Transactions;

public class PaymentUpdateRequest : PaymentCreateRequest
{
    [Required(ErrorMessage = Annotations.FieldIsRequired)]
    public int Id { get; set; }
}