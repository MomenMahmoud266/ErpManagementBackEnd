using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Domain.Interfaces.Repositories.Shared;

namespace ErpManagement.Domain.Interfaces.Repositories.Transactions;

public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<IEnumerable<Payment>> GetBySaleIdAsync(int saleId);
    Task<IEnumerable<Payment>> GetByPurchaseIdAsync(int purchaseId);
}