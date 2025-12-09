using ErpManagement.Domain.DTOs.Request.People.Biller;
using ErpManagement.Domain.DTOs.Response.People.Biller;
using ErpManagement.Domain.Models.People;
using ErpManagement.Services.IServices.People;

namespace ErpManagement.Services.Services.People;

public class BillerService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                          IHubContext<BroadcastHub, IHubClient> hubContext) : IBillerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Biller.GetSpecificSelectAsync(x => x.IsActive,
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

    public async Task<Response<BillerGetAllResponse>> GetAllAsync(RequestLangEnum lang, BillerGetAllFiltrationsRequest model)
    {
        var result = new BillerGetAllResponse
        {
            TotalRecords = await _unitOfWork.Biller.CountAsync(),

            Items = (await _unitOfWork.Biller.GetSpecificSelectAsync(null,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new BillerPaginatedData
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

    public async Task<Response<BillerCreateRequest>> CreateAsync(BillerCreateRequest model)
    {
        bool isExist = await _unitOfWork.Biller
            .ExistAsync(x => x.BillerCode.Trim().ToLower() == model.BillerCode.Trim().ToLower());

        if (isExist)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Shared.Biller]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        await _unitOfWork.Biller.CreateAsync(new()
        {
            WarehouseId = model.WarehouseId,
            CountryId = model.CountryId,
            BillerCode = model.BillerCode,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Email = model.Email,
            City = model.City,
            Address = model.Address,
            ZipCode = model.ZipCode,
            NidPassportNumber = model.NidPassportNumber,
            DateOfJoin = model.DateOfJoin,
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

    public async Task<Response<BillerGetByIdResponse>> GetByIdAsync(int id)
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
                WarehouseId = obj.WarehouseId,
                CountryId = obj.CountryId,
                BillerCode = obj.BillerCode,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Phone = obj.Phone,
                Email = obj.Email,
                City = obj.City,
                Address = obj.Address,
                ZipCode = obj.ZipCode,
                NidPassportNumber = obj.NidPassportNumber,
                DateOfJoin = obj.DateOfJoin,
                ImagePath = obj.ImagePath,
                IsActive = obj.IsActive
            }
        };
    }

    public async Task<Response<BillerUpdateRequest>> UpdateAsync(int id, BillerUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Biller]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        obj.WarehouseId = model.WarehouseId;
        obj.CountryId = model.CountryId;
        obj.BillerCode = model.BillerCode;
        obj.FirstName = model.FirstName;
        obj.LastName = model.LastName;
        obj.Phone = model.Phone;
        obj.Email = model.Email;
        obj.City = model.City;
        obj.Address = model.Address;
        obj.ZipCode = model.ZipCode;
        obj.NidPassportNumber = model.NidPassportNumber;
        obj.DateOfJoin = model.DateOfJoin;
        obj.ImagePath = model.ImagePath;

        _unitOfWork.Biller.Update(obj);
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
                _sharLocalizer[Localization.Shared.Biller]);

            return new Response<string>()
            {
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Biller.Update(obj);

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
                _sharLocalizer[Localization.Shared.Biller]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        _unitOfWork.Biller.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Deleted],
            IsSuccess = true
        };
    }

    #region Private methods
    private async Task<Biller?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Biller.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}