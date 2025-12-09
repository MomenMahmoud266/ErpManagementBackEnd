using ErpManagement.Domain.DTOs.Request.Organization.Company;
using ErpManagement.Domain.DTOs.Response.Organization.Company;
using ErpManagement.Domain.Models.Organization;
using ErpManagement.Services.IServices.Organization;

namespace ErpManagement.Services.Services.Organization;

public class CompanyService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                           IHubContext<BroadcastHub, IHubClient> hubContext) : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Companies.GetSpecificSelectAsync(x => x.IsActive,
            select: x => new SelectListResponse
            {
                Id = x.Id,
                Name = x.Name
            }, orderBy: x => x.OrderByDescending(x => x.Id));

        if (!result.Any())
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Data = Array.Empty<SelectListResponse>(), Message = message, Error = message };
        }

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<CompanyGetAllResponse>> GetAllAsync(RequestLangEnum lang, CompanyGetAllFiltrationsRequest model)
    {
        var result = new CompanyGetAllResponse
        {
            TotalRecords = await _unitOfWork.Companies.CountAsync(),
            Items = (await _unitOfWork.Companies.GetSpecificSelectAsync(null,
                pageNumber: model.PageNumber,
                pageSize: model.PageSize,
                select: x => new CompanyPaginatedData
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                }, orderBy: x => x.OrderByDescending(x => x.Id))).ToList()
        };

        if (result.TotalRecords is 0)
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Data = new() { Items = [] }, Message = message, Error = message };
        }

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<CompanyCreateRequest>> CreateAsync(CompanyCreateRequest model)
    {
        bool isExist = await _unitOfWork.Companies
            .ExistAsync(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower());

        if (isExist)
        {
            string message = string.Format(_sharLocalizer[Localization.IsExist], _sharLocalizer[Localization.Company]);
            return new() { Message = message, Error = message };
        }

        await _unitOfWork.Companies.CreateAsync(new Company
        {
            Name = model.Name,
            Description = model.Description,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address
        });

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Done], IsSuccess = true, Data = model };
    }

    public async Task<Response<CompanyGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string message = _sharLocalizer[Localization.NotFoundData];
            return new() { Message = message, Error = message };
        }

        return new() { IsSuccess = true, Data = _mapper.Map<CompanyGetByIdResponse>(obj) };
    }

    public async Task<Response<CompanyUpdateRequest>> UpdateAsync(int id, CompanyUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Company]);
            return new() { Message = message, Error = message };
        }

        obj.Name = model.Name;
        obj.Description = model.Description;
        obj.Email = model.Email;
        obj.Phone = model.Phone;
        obj.Address = model.Address;

        _unitOfWork.Companies.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Updated], IsSuccess = true, Data = model };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Company]);
            return new() { Error = message, Message = message };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Companies.Update(obj);

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
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Company]);
            return new() { Message = message, Error = message };
        }

        _unitOfWork.Companies.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Message = _sharLocalizer[Localization.Deleted], IsSuccess = true };
    }

    #region Private
    private async Task<Company?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Companies.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}