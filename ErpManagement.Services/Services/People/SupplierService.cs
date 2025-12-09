using ErpManagement.Domain.DTOs.Request.People.Supplier;
using ErpManagement.Domain.DTOs.Response.People.Supplier;
using ErpManagement.Domain.Models.People;
using ErpManagement.Services.IServices.People;

namespace ErpManagement.Services.Services.People;

public class SupplierService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                             IHubContext<BroadcastHub, IHubClient> hubContext) : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Supplier.GetSpecificSelectAsync(x => x.IsActive,
            select: x => new SelectListResponse
            {
                Id = x.Id,
                Name = x.FirstName
            }, orderBy: x =>
            x.OrderByDescending(x => x.Id));

        if (!result.Any())
        {
            string resultIsSuccess = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Data = [],
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        return new()
        {
            Data = result,
            IsSuccess = true
        };
    }

    public async Task<Response<SupplierGetAllResponse>> GetAllAsync(RequestLangEnum lang, SupplierGetAllFiltrationsRequest model)
    {
        var result = new SupplierGetAllResponse
        {
            TotalRecords = await _unitOfWork.Supplier.CountAsync(),

            Items = (await _unitOfWork.Supplier.GetSpecificSelectAsync(null,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new SupplierPaginatedData
            {
                Id = x.Id,
                Name = x.FirstName,
                IsActive = x.IsActive
            }, orderBy: x =>
            x.OrderByDescending(x => x.Id))).ToList()
        };

        if (result.TotalRecords is 0)
        {
            string resultIsSuccess = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Data = new()
                {
                    Items = []
                },
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        return new()
        {
            Data = result,
            IsSuccess = true
        };
    }

    public async Task<Response<SupplierCreateRequest>> CreateAsync(SupplierCreateRequest model)
    {
        bool isExist = await _unitOfWork.Supplier
            .ExistAsync(x => x.SupplierCode.Trim().ToLower() == model.SupplierCode.Trim().ToLower());

        if (isExist)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Shared.Supplier]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        await _unitOfWork.Supplier.CreateAsync(new()
        {
            CountryId = model.CountryId,
            CompanyId = model.CompanyId,
            SupplierCode = model.SupplierCode,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Email = model.Email,
            City = model.City,
            Address = model.Address,
            ZipCode = model.ZipCode,
            ImagePath = model.ImagePath
        });
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Done],
            IsSuccess = true,
            Data = model
        };
    }

    public async Task<Response<SupplierGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string resultIsSuccess = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        return new()
        {
            IsSuccess = true,
            Data = new()
            {
                Id = id,
                CountryId = obj.CountryId,
                CompanyId = obj.CompanyId,
                SupplierCode = obj.SupplierCode,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Phone = obj.Phone,
                Email = obj.Email,
                City = obj.City,
                Address = obj.Address,
                ZipCode = obj.ZipCode,
                ImagePath = obj.ImagePath,
                IsActive = obj.IsActive
            }
        };
    }

    public async Task<Response<SupplierUpdateRequest>> UpdateAsync(int id, SupplierUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Supplier]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        obj.CountryId = model.CountryId;
        obj.CompanyId = model.CompanyId;
        obj.SupplierCode = model.SupplierCode;
        obj.FirstName = model.FirstName;
        obj.LastName = model.LastName;
        obj.Phone = model.Phone;
        obj.Email = model.Email;
        obj.City = model.City;
        obj.Address = model.Address;
        obj.ZipCode = model.ZipCode;
        obj.ImagePath = model.ImagePath;

        _unitOfWork.Supplier.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Updated],
            IsSuccess = true,
            Data = model
        };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Supplier]);

            return new Response<string>()
            {
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Supplier.Update(obj);

        return new Response<string>()
        {
            IsSuccess = await _unitOfWork.CompleteAsync() > 0,
            IsActive = obj.IsActive,
            Message = obj.IsActive
            ? _sharLocalizer[Localization.Activated]
            : _sharLocalizer[Localization.DeActivated]
        };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Supplier]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        _unitOfWork.Supplier.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Deleted],
            IsSuccess = true
        };
    }

    #region Private methods
    private async Task<Supplier?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Supplier.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}