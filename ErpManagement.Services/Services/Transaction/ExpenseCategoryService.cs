using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class ExpenseCategoryService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                                   IHubContext<BroadcastHub, IHubClient> hubContext) : IExpenseCategoryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var items = await _unitOfWork.ExpenseCategories.GetSpecificSelectAsync(
            x => x.IsActive,
            select: x => new SelectListResponse { Id = x.Id, Name = lang == RequestLangEnum.Ar ? x.NameAr : lang == RequestLangEnum.Tr ? x.NameTr : x.NameEn },
            orderBy: q => q.OrderByDescending(x => x.Id));

        if (!items.Any())
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Data = Array.Empty<SelectListResponse>(), Message = message, Error = message };
        }

        return new() { Data = items, IsSuccess = true };
    }

    public async Task<Response<ExpenseCategoryGetAllResponse>> GetAllAsync(RequestLangEnum lang, ExpenseCategoryGetAllFiltrationsForExpenseCategoriesRequest model)
    {
        var total = await _unitOfWork.ExpenseCategories.CountAsync();
        
        var items = (await _unitOfWork.ExpenseCategories.GetSpecificSelectAsync(
            null,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new PaginatedExpenseCategoriesData
            {
                Id = x.Id,
                Name = lang == RequestLangEnum.Ar ? x.NameAr : lang == RequestLangEnum.Tr ? x.NameTr : x.NameEn,
                IsActive = x.IsActive
            },
            orderBy: q => q.OrderByDescending(x => x.Id)
        )).ToList();

        if (total is 0)
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Data = new ExpenseCategoryGetAllResponse { Items = [], TotalRecords = 0 }, Message = message, Error = message };
        }

        return new() { Data = new ExpenseCategoryGetAllResponse { Items = items, TotalRecords = total }, IsSuccess = true };
    }

    public async Task<Response<ExpenseCategoryCreateRequest>> CreateAsync(ExpenseCategoryCreateRequest model)
    {
        bool isExist = await _unitOfWork.ExpenseCategories
            .ExistAsync(x => x.NameEn.Trim().ToLower() == model.NameEn.Trim().ToLower() &&
                             x.NameAr.Trim().ToLower() == model.NameAr.Trim().ToLower());

        if (isExist)
            return new() { Message = _sharLocalizer[Localization.IsExist] };

        var entity = _mapper.Map<ExpenseCategory>(model);
        await _unitOfWork.ExpenseCategories.CreateAsync(entity);
        await _unitOfWork.CompleteAsync();

        await _hubContext.Clients.All.BroadcastMessage();
        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Activated] };
    }

    private async Task<ExpenseCategory?> GetObjByIdAsync(int id) =>
        await _unitOfWork.ExpenseCategories.GetFirstOrDefaultAsync(x => x.Id == id);

    public async Task<Response<ExpenseCategoryGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        return new() { Data = _mapper.Map<ExpenseCategoryGetByIdResponse>(obj), IsSuccess = true };
    }

    public async Task<Response<ExpenseCategoryUpdateRequest>> UpdateAsync(int id, ExpenseCategoryUpdateRequest model)
    {
        if (id != model.Id) return new() { Message = "_sharLocalizer[Localization.InvalidId]" };

        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };

        // duplicate check (skip current)
        bool isExist = await _unitOfWork.ExpenseCategories
            .ExistAsync(x => x.Id != id && x.NameEn.Trim().ToLower() == model.NameEn.Trim().ToLower());
        if (isExist) return new() { Message = _sharLocalizer[Localization.IsExist] };

        obj.NameAr = model.NameAr;
        obj.NameEn = model.NameEn;
        obj.NameTr = model.NameTr;

        _unitOfWork.ExpenseCategories.Update(obj);
        await _unitOfWork.CompleteAsync();

        await _hubContext.Clients.All.BroadcastMessage();
        return new() { Data = model, IsSuccess = true, Message = "_sharLocalizer[Localization.UpdatedSuccess]" };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.NotFoundData] };
        obj.IsActive = !obj.IsActive;
        _unitOfWork.ExpenseCategories.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();
        return new() { Data = "", IsSuccess = true, Message = "_sharLocalizer[Localization.UpdatedSuccess]" };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = "_sharLocalizer[Localization.NotFoundData]" };
        _unitOfWork.ExpenseCategories.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();
        return new() { Data = "", IsSuccess = true, Message = "_sharLocalizer[Localization.DeletedSuccess]" };
    }
}