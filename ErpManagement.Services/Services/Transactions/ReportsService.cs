using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;
using ErpManagement.Domain.Interfaces;

namespace ErpManagement.Services.Services.Transactions;

public class ReportsService(IUnitOfWork unitOfWork, ICurrentTenant currentTenant) : IReportsService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentTenant _currentTenant = currentTenant;

    public async Task<Response<ProfitLossResponse>> GetProfitLossAsync(RequestLangEnum lang, ProfitLossRequest model)
    {
        if (model.From > model.To)
            return new() { Error = "From date must be before To date.", Message = "From date must be before To date." };

        // Get tenant inventory settings
        var tenant = await _unitOfWork.Tenants.GetFirstOrDefaultAsync(x => x.Id == _currentTenant.TenantId);
        string inventoryMode = tenant?.InventoryMode ?? "Perpetual";

        // ── Revenue ──────────────────────────────────────────────────────────
        var salesQuery = await _unitOfWork.Sales.GetAllAsync(
            x => x.TenantId == _currentTenant.TenantId &&
                 x.SaleDate >= model.From &&
                 x.SaleDate <= model.To &&
                 (!model.BranchId.HasValue || x.BranchId == model.BranchId),
            includeProperties: "Items,Payments");

        var sales = salesQuery.ToList();

        decimal revenue;
        if (model.IncludeUnpaidSales)
        {
            revenue = sales.Sum(s => s.TotalAmount);
        }
        else
        {
            // Only sum payments received (paid amounts) within period
            revenue = sales.SelectMany(s => s.Payments)
                .Where(p => p.PaymentDate >= model.From && p.PaymentDate <= model.To)
                .Sum(p => p.PaidAmount);
        }

        // ── Daily revenue breakdown ───────────────────────────────────────────
        var dailyRevenue = sales
            .GroupBy(s => s.SaleDate.Date)
            .Select(g => new DailyRevenueDto
            {
                Date = g.Key,
                Amount = model.IncludeUnpaidSales
                    ? g.Sum(s => s.TotalAmount)
                    : g.SelectMany(s => s.Payments).Sum(p => p.PaidAmount)
            })
            .OrderBy(d => d.Date)
            .ToList();

        // ── Expenses ─────────────────────────────────────────────────────────
        var expensesQuery = await _unitOfWork.Expenses.GetAllAsync(
            x => x.TenantId == _currentTenant.TenantId &&
                 x.ExpenseDate >= model.From &&
                 x.ExpenseDate <= model.To,
            includeProperties: "ExpenseCategory,Warehouse.Branch");

        var expenses = expensesQuery.ToList();

        // Optional: filter by branch via warehouse
        if (model.BranchId.HasValue)
            expenses = expenses.Where(e => e.Warehouse?.BranchId == model.BranchId).ToList();

        decimal totalExpenses = expenses.Sum(e => e.Amount);

        var expensesByCategory = expenses
            .GroupBy(e => e.ExpenseCategory?.NameEn ?? "Uncategorized")
            .Select(g => new ExpenseByCategoryDto { Category = g.Key, Amount = g.Sum(e => e.Amount) })
            .OrderByDescending(e => e.Amount)
            .ToList();

        // ── COGS ─────────────────────────────────────────────────────────────
        decimal cogs = 0;
        string cogsSource;

        if (inventoryMode == "Perpetual")
        {
            // Perpetual (جرد مستمر): sum(Quantity * CostAtSale) for sale items in period
            var saleIds = sales.Select(s => s.Id).ToList();

            if (saleIds.Any())
            {
                cogs = sales
                    .SelectMany(s => s.Items)
                    .Sum(i => i.Quantity * i.CostAtSale);
            }

            cogsSource = "Perpetual-Average";
        }
        else
        {
            // Periodic (جرد دوري): aggregate CogsValue from closed periods overlapping date range
            var closedPeriods = await _unitOfWork.InventoryPeriods.GetAllAsync(
                x => x.TenantId == _currentTenant.TenantId &&
                     x.IsClosed &&
                     x.From >= model.From &&
                     x.To <= model.To &&
                     (!model.BranchId.HasValue || x.BranchId == model.BranchId));

            var periodList = closedPeriods.ToList();

            if (periodList.Any())
            {
                cogs = periodList.Sum(p => p.CogsValue);
                cogsSource = "Periodic";
            }
            else
            {
                cogsSource = "Periodic-NoPeriodsClosed";
            }
        }

        decimal grossProfit = revenue - cogs;
        decimal netProfit = grossProfit - totalExpenses;

        return new()
        {
            IsSuccess = true,
            Data = new ProfitLossResponse
            {
                From = model.From,
                To = model.To,
                Revenue = revenue,
                Cogs = cogs,
                GrossProfit = grossProfit,
                Expenses = totalExpenses,
                NetProfit = netProfit,
                DailyRevenue = dailyRevenue,
                ExpensesByCategory = expensesByCategory,
                InventoryMode = inventoryMode,
                CogsSource = cogsSource
            }
        };
    }
}
