using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
        : base(context, currentTenant)
    {
    }

    public async Task<IEnumerable<Payment>> GetBySaleIdAsync(int saleId) =>
        await GetAllAsync(x => x.SaleId == saleId);

    public async Task<IEnumerable<Payment>> GetByPurchaseIdAsync(int purchaseId) =>
        await GetAllAsync(x => x.PurchaseId == purchaseId);
}