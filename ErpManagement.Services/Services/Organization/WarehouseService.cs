using ErpManagement.Domain.DTOs.Request.Organization.Warehouse;
using ErpManagement.Domain.DTOs.Response.Organization.Warehouse;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Services.IServices.Organization;
using System.Linq.Expressions;

namespace ErpManagement.Services.Services.Organization;

public class WarehouseService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                             IHubContext<BroadcastHub, IHubClient> hubContext) : IWarehouseService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Warehouses.GetSpecificSelectAsync(
            x => x.IsActive,
            select: x => new SelectListResponse
            {
                Id = x.Id,
                Name = x.Name
            },
            orderBy: x => x.OrderByDescending(x => x.Id));

        if (!result.Any())
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Data = Array.Empty<SelectListResponse>(), Message = message, Error = message };
        }

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<WarehouseGetAllResponse>> GetAllAsync(RequestLangEnum lang, WarehouseGetAllFiltrationsRequest model)
    {
        // Optional filter by BranchId if present on the request
        Expression<Func<Warehouse, bool>>? filter = null;

        if (model.BranchId.HasValue)
        {
            var branchId = model.BranchId.Value;
            filter = x => x.BranchId == branchId;
        }

        var result = new WarehouseGetAllResponse
        {
            TotalRecords = await _unitOfWork.Warehouses.CountAsync(filter),
            Items = (await _unitOfWork.Warehouses.GetSpecificSelectAsync(
                filter,
                pageNumber: model.PageNumber,
                pageSize: model.PageSize,
                select: x => new WarehousePaginatedData
                {
                    Id = x.Id,
                    Name = x.Name,
                    BranchId = x.BranchId,
                    BranchName = x.Branch.NameEn,
                    IsActive = x.IsActive
                },
                orderBy: x => x.OrderByDescending(x => x.Id),
                includeProperties: "Branch"
            )).ToList()
        };

        if (result.TotalRecords is 0)
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Data = new() { Items = [] }, Message = message, Error = message };
        }

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<WarehouseCreateRequest>> CreateAsync(WarehouseCreateRequest model)
    {
        bool isExist = await _unitOfWork.Warehouses
            .ExistAsync(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()
                             && x.BranchId == model.BranchId);

        if (isExist)
        {
            string message = string.Format(_sharLocalizer[Localization.IsExist], _sharLocalizer[Localization.Shared.Warehouse]);
            return new() { Message = message, Error = message };
        }

        // Validate Branch exists
        bool branchExists = await _unitOfWork.Branches.ExistAsync(x => x.Id == model.BranchId && x.IsActive);
        if (!branchExists)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.CompanyBranch]);
            return new() { Message = message, Error = message };
        }

        await _unitOfWork.Warehouses.CreateAsync(new Warehouse
        {
            BranchId = model.BranchId,
            Name = model.Name,
            Phone = model.Phone,
            Email = model.Email,
            Address = model.Address,
            Description = model.Description,
            IsActive = true
        });

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Done], IsSuccess = true, Data = model };
    }

    public async Task<Response<WarehouseGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await _unitOfWork.Warehouses.GetFirstOrDefaultAsync(
            x => x.Id == id,
            includeProperties: "Branch");

        if (obj is null)
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Message = message, Error = message };
        }

        var dto = _mapper.Map<WarehouseGetByIdResponse>(obj);
        return new() { IsSuccess = true, Data = dto };
    }

    public async Task<Response<WarehouseUpdateRequest>> UpdateAsync(int id, WarehouseUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Warehouse]);
            return new() { Message = message, Error = message };
        }

        // Validate Branch exists if it's being changed
        if (obj.BranchId != model.BranchId)
        {
            bool branchExists = await _unitOfWork.Branches.ExistAsync(x => x.Id == model.BranchId && x.IsActive);
            if (!branchExists)
            {
                string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.CompanyBranch]);
                return new() { Message = message, Error = message };
            }
        }

        obj.Name = model.Name;
        obj.BranchId = model.BranchId;
        obj.Phone = model.Phone;
        obj.Email = model.Email;
        obj.Address = model.Address;
        obj.Description = model.Description;

        _unitOfWork.Warehouses.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Updated], IsSuccess = true, Data = model };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Warehouse]);
            return new() { Error = message, Message = message };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Warehouses.Update(obj);

        return new()
        {
            IsSuccess = await _unitOfWork.CompleteAsync() > 0,
            IsActive = obj.IsActive,
            Message = obj.IsActive ? _sharLocalizer[Localization.Activated] : _sharLocalizer[Localization.DeActivated]
        };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Warehouse]);
            return new() { Message = message, Error = message };
        }

        // ✅ Prevent deleting warehouse that has stock
        bool hasStock = await _unitOfWork.WarehouseProducts
            .ExistAsync(x => x.WarehouseId == id);

        if (hasStock)
        {
            string message = string.Format(
                _sharLocalizer[Localization.CannotDeleteHasChildren],
                _sharLocalizer[Localization.Shared.Warehouse],
                _sharLocalizer[Localization.Shared.Stock]
            );

            return new() { Message = message, Error = message };
        }

        _unitOfWork.Warehouses.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Deleted], IsSuccess = true };
    }

    #region Private
    private async Task<Warehouse?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Warehouses.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}
