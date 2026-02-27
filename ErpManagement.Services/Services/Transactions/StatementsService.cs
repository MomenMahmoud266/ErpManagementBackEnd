using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class StatementsService(IUnitOfWork unitOfWork) : IStatementsService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Response<CustomerStatementResponse>> GetCustomerStatementAsync(
        RequestLangEnum lang, CustomerStatementRequest model)
    {
        bool customerExists = await _unitOfWork.Customer.ExistAsync(x => x.Id == model.CustomerId);
        if (!customerExists)
            return new() { Error = "Customer not found.", Message = "Customer not found." };

        var customer = await _unitOfWork.Customer.GetByIdAsync(model.CustomerId);

        // --- Collect all raw data -------------------------------------------------

        // Sales for this customer (optionally filtered by branch)
        var allSales = (await _unitOfWork.Sales.GetAllAsync(
            x => x.CustomerId == model.CustomerId &&
                 (!model.BranchId.HasValue || x.BranchId == model.BranchId)))
            .ToList();

        var saleIds = allSales.Select(s => s.Id).ToList();

        // Payments linked to any of these sales
        var allPayments = (await _unitOfWork.Payments.GetAllAsync(
            x => x.SaleId.HasValue && saleIds.Contains(x.SaleId.Value)))
            .ToList();

        // Sale returns for this customer
        var allReturns = (await _unitOfWork.SaleReturns.GetAllAsync(
            x => x.CustomerId == model.CustomerId &&
                 saleIds.Contains(x.SaleId)))
            .ToList();

        // --- Opening balance (before 'From') --------------------------------------
        decimal openingBalance = ComputeCustomerBalance(
            allSales.Where(s => s.SaleDate < model.From).ToList(),
            allPayments.Where(p => p.PaymentDate < model.From).ToList(),
            allReturns.Where(r => r.ReturnDate < model.From).ToList());

        // --- Period data ----------------------------------------------------------
        var periodSales = allSales.Where(s => s.SaleDate >= model.From && s.SaleDate <= model.To).ToList();
        var periodPayments = allPayments.Where(p => p.PaymentDate >= model.From && p.PaymentDate <= model.To).ToList();
        var periodReturns = allReturns.Where(r => r.ReturnDate >= model.From && r.ReturnDate <= model.To).ToList();

        // --- Build lines ---------------------------------------------------------
        var rawLines = new List<(DateTime date, string type, string reference, decimal debit, decimal credit)>();

        foreach (var s in periodSales)
            rawLines.Add((s.SaleDate, "Sale", s.SaleCode, s.TotalAmount, 0));

        foreach (var p in periodPayments)
            rawLines.Add((p.PaymentDate, "Payment", p.PaymentCode, 0, p.PaidAmount));

        foreach (var r in periodReturns)
            rawLines.Add((r.ReturnDate, "Return", r.ReturnCode, 0, r.TotalAmount));

        var sortedLines = rawLines.OrderBy(l => l.date).ThenBy(l => l.type).ToList();

        decimal runningBalance = openingBalance;
        var lines = sortedLines.Select(l =>
        {
            runningBalance += l.debit - l.credit;
            return new CustomerStatementLineDto
            {
                Date = l.date,
                Type = l.type,
                Reference = l.reference,
                Debit = l.debit,
                Credit = l.credit,
                Balance = runningBalance
            };
        }).ToList();

        return new()
        {
            Data = new CustomerStatementResponse
            {
                CustomerId = model.CustomerId,
                CustomerName = customer?.FirstName ?? string.Empty,
                OpeningBalance = openingBalance,
                TotalSales = periodSales.Sum(s => s.TotalAmount),
                TotalPayments = periodPayments.Sum(p => p.PaidAmount),
                TotalReturns = periodReturns.Sum(r => r.TotalAmount),
                ClosingBalance = runningBalance,
                Lines = lines
            },
            IsSuccess = true
        };
    }

    public async Task<Response<SupplierStatementResponse>> GetSupplierStatementAsync(
        RequestLangEnum lang, SupplierStatementRequest model)
    {
        bool supplierExists = await _unitOfWork.Supplier.ExistAsync(x => x.Id == model.SupplierId);
        if (!supplierExists)
            return new() { Error = "Supplier not found.", Message = "Supplier not found." };

        var supplier = await _unitOfWork.Supplier.GetByIdAsync(model.SupplierId);

        // --- Collect all raw data -------------------------------------------------

        var allPurchases = (await _unitOfWork.Purchases.GetAllAsync(
            x => x.SupplierId == model.SupplierId))
            .ToList();

        var purchaseIds = allPurchases.Select(p => p.Id).ToList();

        // Payments linked to purchases
        var allPayments = (await _unitOfWork.Payments.GetAllAsync(
            x => x.PurchaseId.HasValue && purchaseIds.Contains(x.PurchaseId.Value)))
            .ToList();

        // Purchase returns
        var allReturns = (await _unitOfWork.PurchaseReturns.GetAllAsync(
            x => x.SupplierId == model.SupplierId &&
                 (!x.PurchaseId.HasValue || purchaseIds.Contains(x.PurchaseId.Value))))
            .ToList();

        // --- Opening balance (before 'From') --------------------------------------
        decimal openingBalance = ComputeSupplierBalance(
            allPurchases.Where(p => p.PurchaseDate < model.From).ToList(),
            allPayments.Where(p => p.PaymentDate < model.From).ToList(),
            allReturns.Where(r => r.ReturnDate < model.From).ToList());

        // --- Period data ----------------------------------------------------------
        var periodPurchases = allPurchases.Where(p => p.PurchaseDate >= model.From && p.PurchaseDate <= model.To).ToList();
        var periodPayments = allPayments.Where(p => p.PaymentDate >= model.From && p.PaymentDate <= model.To).ToList();
        var periodReturns = allReturns.Where(r => r.ReturnDate >= model.From && r.ReturnDate <= model.To).ToList();

        // --- Build lines ---------------------------------------------------------
        var rawLines = new List<(DateTime date, string type, string reference, decimal debit, decimal credit)>();

        foreach (var p in periodPurchases)
            rawLines.Add((p.PurchaseDate, "Purchase", p.PurchaseCode, p.TotalAmount, 0));

        foreach (var pay in periodPayments)
            rawLines.Add((pay.PaymentDate, "Payment", pay.PaymentCode, 0, pay.PaidAmount));

        foreach (var r in periodReturns)
            rawLines.Add((r.ReturnDate, "Return", r.ReturnCode, 0, r.TotalAmount));

        var sortedLines = rawLines.OrderBy(l => l.date).ThenBy(l => l.type).ToList();

        decimal runningBalance = openingBalance;
        var lines = sortedLines.Select(l =>
        {
            runningBalance += l.debit - l.credit;
            return new SupplierStatementLineDto
            {
                Date = l.date,
                Type = l.type,
                Reference = l.reference,
                Debit = l.debit,
                Credit = l.credit,
                Balance = runningBalance
            };
        }).ToList();

        return new()
        {
            Data = new SupplierStatementResponse
            {
                SupplierId = model.SupplierId,
                SupplierName = supplier?.FirstName ?? string.Empty,
                OpeningBalance = openingBalance,
                TotalPurchases = periodPurchases.Sum(p => p.TotalAmount),
                TotalPayments = periodPayments.Sum(p => p.PaidAmount),
                TotalReturns = periodReturns.Sum(r => r.TotalAmount),
                ClosingBalance = runningBalance,
                Lines = lines
            },
            IsSuccess = true
        };
    }

    // ── Helpers ─────────────────────────────────────────────────────────────────

    private static decimal ComputeCustomerBalance(
        IList<ErpManagement.Domain.Models.Transactions.Sale> sales,
        IList<ErpManagement.Domain.Models.Transactions.Payment> payments,
        IList<ErpManagement.Domain.Models.Transactions.SaleReturn> returns)
        => sales.Sum(s => s.TotalAmount)
           - payments.Sum(p => p.PaidAmount)
           - returns.Sum(r => r.TotalAmount);

    private static decimal ComputeSupplierBalance(
        IList<ErpManagement.Domain.Models.Transactions.Purchase> purchases,
        IList<ErpManagement.Domain.Models.Transactions.Payment> payments,
        IList<ErpManagement.Domain.Models.Transactions.PurchaseReturn> returns)
        => purchases.Sum(p => p.TotalAmount)
           - payments.Sum(p => p.PaidAmount)
           - returns.Sum(r => r.TotalAmount);
}
