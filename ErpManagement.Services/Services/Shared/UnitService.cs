namespace ErpManagement.Services.Services.Shared;

public class UnitService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                            IHubContext<BroadcastHub, IHubClient> hubContext) : IUnitService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Units.GetSpecificSelectAsync(x => x.IsActive,
            select: x => new SelectListResponse
            {
                Id = x.Id,
                Name = x.Name
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

    public async Task<Response<UnitGetAllResponse>> GetAllAsync(RequestLangEnum lang, UnitGetAllFiltrationsRequest model)
    {
        var result = new UnitGetAllResponse
        {
            TotalRecords = await _unitOfWork.Units.CountAsync(),

            Items = (await _unitOfWork.Units.GetSpecificSelectAsync(null,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new UnitPaginatedData
            {
                Id = x.Id,
                Name = x.Name,
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

    public async Task<Response<UnitCreateRequest>> CreateAsync(UnitCreateRequest model)
    {
        bool isExist = await _unitOfWork.Units
            .ExistAsync(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower());

        if (isExist)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Shared.Unit]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        await _unitOfWork.Units.CreateAsync(new()
        {
            Name = model.Name,
            UnitType = model.UnitType,
            Symbol = model.Symbol
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

    public async Task<Response<UnitGetByIdResponse>> GetByIdAsync(int id)
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
                Name = obj.Name,
                UnitType = obj.UnitType,
                Symbol = obj.Symbol
            }
        };
    }

    public async Task<Response<UnitUpdateRequest>> UpdateAsync(int id, UnitUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Unit]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        obj.Name = model.Name;
        obj.UnitType = model.UnitType;
        obj.Symbol = model.Symbol;

        _unitOfWork.Units.Update(obj);
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
                _sharLocalizer[Localization.Shared.Unit]);

            return new Response<string>()
            {
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Units.Update(obj);

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
                _sharLocalizer[Localization.Shared.Unit]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        _unitOfWork.Units.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Deleted],
            IsSuccess = true
        };
    }

    #region Private methods
    private async Task<Unit?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Units.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}