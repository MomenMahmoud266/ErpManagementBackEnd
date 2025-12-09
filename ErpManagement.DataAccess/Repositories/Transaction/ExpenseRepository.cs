using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Interfaces.Repositories.Transactions;
using ErpManagement.Domain.Models.Transactions;

namespace ErpManagement.DataAccess.Repositories.Transactions;

public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
{
    public ExpenseRepository(ErpManagementDbContext context, ICurrentTenant currentTenant)
        : base(context, currentTenant)
    {
    }
}