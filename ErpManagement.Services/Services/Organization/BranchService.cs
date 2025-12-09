using ErpManagement.Domain.DTOs.Request.Organization.Branch;
using ErpManagement.Domain.DTOs.Response.Organization.Branch;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Services.IServices.Organization;

namespace ErpManagement.Services.Services.Organization;

public class BranchService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                          IHubContext<BroadcastHub, IHubClient> hubContext, ICurrentTenant currentTenant) : IBranchService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;
    private readonly ICurrentTenant _currentTenant = currentTenant;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Branches.GetSpecificSelectAsync(
            x => x.IsActive,
            select: x => new SelectListResponse
            {
                Id = x.Id,
                Name = lang == RequestLangEnum.En ? x.NameEn : x.NameAr // TODO: switch by lang if needed
            },
            orderBy: x => x.OrderByDescending(x => x.Id));

        if (!result.Any())
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Data = Array.Empty<SelectListResponse>(), Message = message, Error = message };
        }

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<BranchGetAllResponse>> GetAllAsync(RequestLangEnum lang, BranchGetAllFiltrationsRequest model)
    {
        var result = new BranchGetAllResponse
        {
            TotalRecords = await _unitOfWork.Branches.CountAsync(),
            Items = (await _unitOfWork.Branches.GetSpecificSelectAsync(
                null,
                pageNumber: model.PageNumber,
                pageSize: model.PageSize,
                select: x => new BranchPaginatedData
                {
                    Id = x.Id,
                    Name = lang==RequestLangEnum.En? x.NameEn:x.NameAr ?? string.Empty,
                    City = x.City,
                    IsActive = x.IsActive,
                    CountryName= lang == RequestLangEnum.En ? x.Country.NameEn :x.Country.NameAr,
                    Phone = x.Phone
                },
                orderBy: x => x.OrderByDescending(x => x.Id)
            )).ToList()
        };

        if (result.TotalRecords is 0)
        {
            return new() { Data = new() { Items = [] }, Message = string.Empty, Error = string.Empty };
        }

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<BranchCreateRequest>> CreateAsync(BranchCreateRequest model)
    {
        bool isExist = await _unitOfWork.Branches
            .ExistAsync(x => x.NameEn.Trim().ToLower() == model.NameEn.Trim().ToLower() && x.TenantId == _currentTenant.TenantId);

        if (isExist)
        {
            string message = string.Format(_sharLocalizer[Localization.IsExist], _sharLocalizer[Localization.Shared.CompanyBranch]);
            return new() { Message = message, Error = message };
        }

        // Validate Country exists
        bool countryExists = await _unitOfWork.Countries.ExistAsync(x => x.Id == model.CountryId);
        if (!countryExists)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Country]);
            return new() { Message = message, Error = message };
        }

        await _unitOfWork.Branches.CreateAsync(new Branch
        {
            TenantId = _currentTenant.TenantId,
            NameEn = model.NameEn,
            NameAr = model.NameAr,
            NameTr = model.NameTr,
            CountryId = model.CountryId,
            City = model.City,
            ZipCode = model.ZipCode,
            Address = model.Address,
            Phone = model.Phone,
            IsActive = true
        });

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Done], IsSuccess = true, Data = model };
    }

    public async Task<Response<BranchGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Message = message, Error = message };
        }

        return new() { IsSuccess = true, Data = _mapper.Map<BranchGetByIdResponse>(obj) };
    }

    public async Task<Response<BranchUpdateRequest>> UpdateAsync(int id, BranchUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.CompanyBranch]);
            return new() { Message = message, Error = message };
        }

        // Validate Country exists if changed
        if (obj.CountryId != model.CountryId)
        {
            bool countryExists = await _unitOfWork.Countries.ExistAsync(x => x.Id == model.CountryId);
            if (!countryExists)
            {
                string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Country]);
                return new() { Message = message, Error = message };
            }
        }

        obj.NameEn = model.NameEn;
        obj.NameAr = model.NameAr;
        obj.NameTr = model.NameTr;
        obj.CountryId = model.CountryId;
        obj.City = model.City;
        obj.ZipCode = model.ZipCode;
        obj.Address = model.Address;
        obj.Phone = model.Phone;

        _unitOfWork.Branches.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Updated], IsSuccess = true, Data = model };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.CompanyBranch]);
            return new() { Error = message, Message = message };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Branches.Update(obj);

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
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.CompanyBranch]);
            return new() { Message = message, Error = message };
        }

        // ✅ Prevent deleting a branch that has warehouses
        bool hasWarehouses = await _unitOfWork.Warehouses
            .ExistAsync(x => x.BranchId == id);

        if (hasWarehouses)
        {
            string message = string.Format(
                _sharLocalizer[Localization.CannotDeleteHasChildren],
                _sharLocalizer[Localization.Shared.CompanyBranch],
                _sharLocalizer[Localization.Shared.Warehouse]
            );

            return new() { Message = message, Error = message };
        }

        _unitOfWork.Branches.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Deleted], IsSuccess = true };
    }

    #region Private
    private async Task<Branch?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Branches.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}
