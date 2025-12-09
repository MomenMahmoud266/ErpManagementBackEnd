namespace ErpManagement.Services.Services.Shared;

public class CountryService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                            IHubContext<BroadcastHub, IHubClient> hubContext) : ICountryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    #region Country

    public async Task<Response<IEnumerable<SelectListResponse>>> ListOfCountriesAsync(RequestLangEnum lang)
    {
        //var list = (await _unitOfWork.Countries.GetFirstOrDefaultAsync(x => x.Id == 7)).ListOfStates;

        var result = await _unitOfWork.Countries.GetSpecificSelectAsync(x => x.IsActive,
            select: x => new SelectListResponse
            {
                Id = x.Id,
                Name = lang == RequestLangEnum.Ar ? x.NameAr : lang == RequestLangEnum.En ? x.NameEn : x.NameTr
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

    //public async Task<Response<SharGetAllCountriesResponse>> GetAllCountriesAsync(RequestLangEnum lang, SharGetAllFiltrationsForCountriesRequest model)
    //{
    //    var result = new SharGetAllCountriesResponse
    //    {
    //        TotalRecords = await _unitOfWork.Countries.CountAsync(),

    //        Items = (await _unitOfWork.Countries.GetSpecificSelectAsync(null,
    //        pageNumber: model.PageNumber,
    //        pageSize: model.PageSize,
    //        select: x => new PaginatedCountriesData
    //        {
    //            Id = x.Id,
    //            Name = lang == RequestLangEnum.Ar ? x.NameAr : lang == RequestLangEnum.En ? x.NameEn : x.NameTr,
    //            IsActive = x.IsActive
    //        }, orderBy: x =>
    //        x.OrderByDescending(x => x.Id))).ToList()
    //    };

    //    if (result.TotalRecords is 0)
    //    {
    //        string resultIsSuccess = _sharLocalizer[Localization.NotFoundData];

    //        return new()
    //        {
    //            Data = new()
    //            {
    //                Items = []
    //            },
    //            Message = resultIsSuccess,
    //            Error = resultIsSuccess
    //        };
    //    }

    //    return new()
    //    {
    //        Data = result,
    //        IsSuccess = true
    //    };
    //}

    //public async Task<Response<SharCreateCountryRequest>> CreateCountryAsync(SharCreateCountryRequest model)
    //{
    //    //bool isExist = await _unitOfWork.Countries
    //    //    .ExistAsync(x => x.NameAr.Equals(model.NameAr, StringComparison.CurrentCultureIgnoreCase)
    //    //    && 
    //    //    x.NameEn.Equals(model.NameAr, StringComparison.CurrentCultureIgnoreCase)
    //    //    &&
    //    //    x.NameTr.Equals(model.NameAr, StringComparison.CurrentCultureIgnoreCase));

    //    bool isExist = await _unitOfWork.Countries
    //        .ExistAsync(x => x.NameAr.Trim().ToLower() == model.NameAr.Trim().ToLower()
    //        &&
    //        x.NameEn.Trim().ToLower() == model.NameEn.Trim().ToLower()
    //        &&
    //        x.NameTr.Trim().ToLower() == model.NameTr.Trim().ToLower());

    //    if (isExist)
    //    {
    //        string resultIsSuccess = string.Format(_sharLocalizer[Localization.IsExist],
    //            _sharLocalizer[Localization.Shared.Country]);

    //        return new()
    //        {
    //            Message = resultIsSuccess,
    //            Error = resultIsSuccess
    //        };
    //    }

    //    await _unitOfWork.Countries.CreateAsync(new()
    //    {
    //        NameAr = model.NameAr,
    //        NameEn = model.NameEn,
    //        NameTr = model.NameTr
    //    });
    //    await _unitOfWork.CompleteAsync();
    //    await _hubContext.Clients.All.BroadcastMessage();

    //    return new()
    //    {
    //        Message = _sharLocalizer[Localization.Done],
    //        IsSuccess = true,
    //        Data = model
    //    };
    //}

    //public async Task<Response<SharGetCountryByIdResponse>> GetCountryByIdAsync(int id)
    //{
    //    var obj = await GetObJByIdAsync(id);

    //    if (obj is null)
    //    {
    //        string resultIsSuccess = _sharLocalizer[Localization.NotFoundData];

    //        return new()
    //        {
    //            Message = resultIsSuccess,
    //            Error = resultIsSuccess
    //        };
    //    }

    //    return new()
    //    {
    //        IsSuccess = true,

    //        Data = new()
    //        {
    //            Id = id,
    //            NameAr = obj.NameAr,
    //            NameEn = obj.NameEn,
    //            NameTr = obj.NameTr
    //        }
    //    };
    //}

    //public async Task<Response<SharUpdateCountryRequest>> UpdateCountryAsync(int id, SharUpdateCountryRequest model)
    //{
    //    var obj = await GetObJByIdAsync(id);

    //    if (obj is null || id != model.Id)
    //    {
    //        string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
    //            _sharLocalizer[Localization.Shared.Country]);

    //        return new()
    //        {
    //            Message = resultIsSuccess,
    //            Error = resultIsSuccess
    //        };
    //    }

    //    obj.NameAr = model.NameAr;
    //    obj.NameEn = model.NameEn;
    //    obj.NameTr = model.NameTr;

    //    _unitOfWork.Countries.Update(obj);
    //    await _unitOfWork.CompleteAsync();
    //    await _hubContext.Clients.All.BroadcastMessage();

    //    return new()
    //    {
    //        Message = _sharLocalizer[Localization.Updated],
    //        IsSuccess = true,
    //        Data = model
    //    };
    //}

    //public async Task<Response<string>> UpdateActiveOrNotCountryAsync(int id)
    //{
    //    var obj = await GetObJByIdAsync(id);

    //    if (obj is null)
    //    {
    //        string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
    //            _sharLocalizer[Localization.Shared.Country]);

    //        return new Response<string>()
    //        {
    //            Error = resultIsSuccess,
    //            Message = resultIsSuccess
    //        };
    //    }

    //    obj.IsActive = !obj.IsActive;
    //    _unitOfWork.Countries.Update(obj);

    //    return new Response<string>()
    //    {
    //        IsSuccess = await _unitOfWork.CompleteAsync() > 0,
    //        IsActive = obj.IsActive,
    //        Message = obj.IsActive
    //        ? _sharLocalizer[Localization.Activated]
    //        : _sharLocalizer[Localization.DeActivated]
    //    };
    //}

    //public async Task<Response<string>> DeleteCountryAsync(int id)
    //{
    //    var obj = await GetObJByIdAsync(id);

    //    if (obj is null)
    //    {
    //        string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
    //            _sharLocalizer[Localization.Shared.Country]);

    //        return new()
    //        {
    //            Message = resultIsSuccess,
    //            Error = resultIsSuccess
    //        };
    //    }

    //    _unitOfWork.Countries.Delete(obj);
    //    await _unitOfWork.CompleteAsync();
    //    await _hubContext.Clients.All.BroadcastMessage();

    //    return new()
    //    {
    //        Message = _sharLocalizer[Localization.Deleted],
    //        IsSuccess = true
    //    };
    //}


    #region Private methods


    #endregion

    #endregion
}


