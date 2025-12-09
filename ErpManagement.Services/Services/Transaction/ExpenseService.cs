using System.Linq.Expressions;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class ExpenseService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                           IHubContext<BroadcastHub, IHubClient> hubContext) : IExpenseService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    private async Task<Expense?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Expenses.GetFirstOrDefaultAsync(x => x.Id == id,
            includeProperties: "ExpenseCategory,Supplier,Branch,Warehouse");

    public async Task<Response<ExpenseGetAllResponse>> GetAllAsync(RequestLangEnum lang, ExpenseGetAllFiltrationsForExpensesRequest model)
    {
        Expression<Func<Expense, bool>> filter = x =>
            (!model.ExpenseCategoryId.HasValue || x.ExpenseCategoryId == model.ExpenseCategoryId.Value) &&
            (!model.SupplierId.HasValue || x.SupplierId == model.SupplierId.Value) &&
            (!model.IsActive.HasValue || x.IsActive == model.IsActive.Value) &&
            (!model.FromDate.HasValue || x.ExpenseDate >= model.FromDate.Value.Date) &&
            (!model.ToDate.HasValue || x.ExpenseDate <= model.ToDate.Value.Date);

        var total = await _unitOfWork.Expenses.CountAsync(filter);

        var items = (await _unitOfWork.Expenses.GetSpecificSelectAsync(
            filter,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new PaginatedExpensesData
            {
                Id = x.Id,
                ExpenseCode = x.ExpenseCode,
                ExpenseCategoryId = x.ExpenseCategoryId,
                CategoryName = x.ExpenseCategory != null ? (lang == RequestLangEnum.Ar ? x.ExpenseCategory.NameAr : lang == RequestLangEnum.Tr ? x.ExpenseCategory.NameTr : x.ExpenseCategory.NameEn) : string.Empty,
                SupplierName = x.Supplier != null ? x.Supplier.FirstName : null,
                ExpenseDate = x.ExpenseDate,
                Amount = x.Amount,
                PaymentMethod = string.Empty,
                IsActive = x.IsActive
            },
            includeProperties: "ExpenseCategory,Supplier",
            orderBy: q => q.OrderByDescending(x => x.Id)
        )).ToList();

        if (total == 0)
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Data = new ExpenseGetAllResponse { Items = [], TotalRecords = 0 }, Message = message, Error = message };
        }

        var result = new ExpenseGetAllResponse
        {
            TotalRecords = total,
            Items = items
        };

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<ExpenseGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };
        
        var dto = _mapper.Map<ExpenseGetByIdResponse>(obj);
        dto.CategoryName = obj.ExpenseCategory != null ? obj.ExpenseCategory.NameEn : string.Empty;
        dto.SupplierName = obj.Supplier?.FirstName;
        dto.BranchName = obj.Warehouse.Branch?.NameEn;
        dto.WarehouseName = obj.Warehouse?.Name;
        
        return new() { Data = dto, IsSuccess = true };
    }

    public async Task<Response<ExpenseCreateRequest>> CreateAsync(ExpenseCreateRequest model)
    {
        // Validate expense category
        var category = await _unitOfWork.ExpenseCategories.GetFirstOrDefaultAsync(x => x.Id == model.ExpenseCategoryId && x.IsActive);
        if (category is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };

        // Validate supplier if provided
        if (model.SupplierId.HasValue)
        {
            var supplier = await _unitOfWork.Supplier.GetFirstOrDefaultAsync(x => x.Id == model.SupplierId.Value && x.IsActive);
            if (supplier is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        }

        // Validate branch if provided
        if (model.BranchId.HasValue)
        {
            var branch = await _unitOfWork.Branches.GetFirstOrDefaultAsync(x => x.Id == model.BranchId.Value && x.IsActive);
            if (branch is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        }

        // Validate warehouse if provided
        if (model.WarehouseId.HasValue)
        {
            var warehouse = await _unitOfWork.Warehouses.GetFirstOrDefaultAsync(x => x.Id == model.WarehouseId.Value && x.IsActive);
            if (warehouse is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        }

        var entity = _mapper.Map<Expense>(model);
        await _unitOfWork.Expenses.CreateAsync(entity);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Activated] };
    }

    public async Task<Response<ExpenseUpdateRequest>> UpdateAsync(int id, ExpenseUpdateRequest model)
    {
        if (id != model.Id) return new() { Message = "_sharLocalizer[Localization.InvalidId]" };

        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };

        // Validate expense category
        var category = await _unitOfWork.ExpenseCategories.GetFirstOrDefaultAsync(x => x.Id == model.ExpenseCategoryId && x.IsActive);
        if (category is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };

        // Validate supplier if provided
        if (model.SupplierId.HasValue)
        {
            var supplier = await _unitOfWork.Supplier.GetFirstOrDefaultAsync(x => x.Id == model.SupplierId.Value && x.IsActive);
            if (supplier is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        }

        // Validate branch if provided
        if (model.BranchId.HasValue)
        {
            var branch = await _unitOfWork.Branches.GetFirstOrDefaultAsync(x => x.Id == model.BranchId.Value && x.IsActive);
            if (branch is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        }

        // Validate warehouse if provided
        if (model.WarehouseId.HasValue)
        {
            var warehouse = await _unitOfWork.Warehouses.GetFirstOrDefaultAsync(x => x.Id == model.WarehouseId.Value && x.IsActive);
            if (warehouse is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        }

        // Update properties
        obj.ExpenseCategoryId = model.ExpenseCategoryId;
        obj.SupplierId = model.SupplierId;
        obj.WarehouseId = model.WarehouseId;
        obj.ExpenseCode = model.ExpenseCode ?? obj.ExpenseCode;
        obj.ExpenseDate = model.ExpenseDate;
        obj.Amount = model.Amount;
        obj.Comment = model.Description; // Note: Expense model uses Comment, not Description

        _unitOfWork.Expenses.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = "_sharLocalizer[Localization.UpdatedSuccess]" };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        
        obj.IsActive = !obj.IsActive;
        _unitOfWork.Expenses.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();
        
        return new() { Data = "", IsSuccess = true, Message = "_sharLocalizer[Localization.UpdatedSuccess]" };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        
        _unitOfWork.Expenses.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();
        
        return new() { Data = "", IsSuccess = true, Message = "_sharLocalizer[Localization.DeletedSuccess]" };
    }
}