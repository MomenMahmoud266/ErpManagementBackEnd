// ErpManagement.Services.Services.Transactions\ExpenseInvoiceService.cs
using System.Linq.Expressions;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class ExpenseInvoiceService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer) : IExpenseInvoiceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;

    public async Task<Response<ExpenseInvoiceGetAllResponse>> GetAllAsync(RequestLangEnum lang, ExpenseInvoiceGetAllFiltrationsForExpenseInvoicesRequest model)
    {
        Expression<Func<Expense, bool>> filter = x =>
            (!model.ExpenseCategoryId.HasValue || x.ExpenseCategoryId == model.ExpenseCategoryId.Value) &&
            (!model.SupplierId.HasValue || x.SupplierId == model.SupplierId.Value) &&
            (!model.WarehouseId.HasValue || (x.WarehouseId.HasValue && x.WarehouseId.Value == model.WarehouseId.Value)) &&
            (string.IsNullOrEmpty(model.ExpenseCode) || x.ExpenseCode.Contains(model.ExpenseCode)) &&
            (!model.FromDate.HasValue || x.ExpenseDate >= model.FromDate.Value) &&
            (!model.ToDate.HasValue || x.ExpenseDate <= model.ToDate.Value) &&
            (!model.IsActive.HasValue || x.IsActive == model.IsActive.Value);

        var total = await _unitOfWork.Expenses.CountAsync(filter);

        var items = (await _unitOfWork.Expenses.GetSpecificSelectAsync(
            filter,
            select: x => new PaginatedExpenseInvoicesData
            {
                Id = x.Id,
                InvoiceCode = x.ExpenseCode,
                ExpenseDate = x.ExpenseDate,
                CategoryName = x.ExpenseCategory != null ? (lang == RequestLangEnum.Ar ? x.ExpenseCategory.NameAr : lang == RequestLangEnum.Tr ? x.ExpenseCategory.NameTr : x.ExpenseCategory.NameEn) : string.Empty,
                SupplierName = x.Supplier != null ? x.Supplier.FirstName : null,
                BranchName = x.Warehouse.Branch != null ? x.Warehouse.Branch.NameEn : null,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : null,
                Amount = x.Amount,
                PaymentMethod = string.Empty,
                IsActive = x.IsActive
            },
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            orderBy: q => q.OrderByDescending(z => z.Id)
        )).ToList();

        if (!items.Any())
        {
            string msg = _sharLocalizer[Localization.NotFoundData];
            return new()
            {
                Data = new ExpenseInvoiceGetAllResponse { Items = [], TotalRecords = 0 },
                Message = msg,
                Error = msg
            };
        }

        return new() { Data = new ExpenseInvoiceGetAllResponse { Items = items, TotalRecords = total }, IsSuccess = true };
    }

    public async Task<Response<ExpenseInvoiceGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await _unitOfWork.Expenses.GetFirstOrDefaultAsync(x => x.Id == id,
            includeProperties: "ExpenseCategory,Supplier,Branch,Warehouse");

        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Data = null!, Message = msg, Error = msg };
        }

        var dto = new ExpenseInvoiceGetByIdResponse
        {
            Id = obj.Id,
            ExpenseCode = obj.ExpenseCode,
            ExpenseDate = obj.ExpenseDate,
            CategoryName = obj.ExpenseCategory != null ? obj.ExpenseCategory.NameEn : string.Empty,
            SupplierName = obj.Supplier?.FirstName,
            BranchName = obj.Warehouse.Branch?.NameEn,
            WarehouseName = obj.Warehouse?.Name,
            Amount = obj.Amount,
            PaymentMethod = string.Empty,
            Description = obj.Comment
        };

        return new() { Data = dto, IsSuccess = true };
    }
}