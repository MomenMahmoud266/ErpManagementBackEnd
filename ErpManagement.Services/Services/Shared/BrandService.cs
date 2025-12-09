namespace ErpManagement.Services.Services.Shared;

public class BrandService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                            IHubContext<BroadcastHub, IHubClient> hubContext) : IBrandService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Brands.GetSpecificSelectAsync(x => x.IsActive,
            select: x => new SelectListResponse
            {
                Id = x.Id,
                Name = x.Title
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

    public async Task<Response<BrandGetAllResponse>> GetAllAsync(RequestLangEnum lang, BrandGetAllFiltrationsRequest model)
    {
        var result = new BrandGetAllResponse
        {
            TotalRecords = await _unitOfWork.Brands.CountAsync(),

            Items = (await _unitOfWork.Brands.GetSpecificSelectAsync(null,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new BrandPaginatedData
            {
                Id = x.Id,
                Name = x.Title,
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

    public async Task<Response<BrandCreateRequest>> CreateAsync(BrandCreateRequest model)
    {
        bool isExist = await _unitOfWork.Brands
            .ExistAsync(x => x.Title.Trim().ToLower() == model.Title.Trim().ToLower());

        if (isExist)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Shared.Brand]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        await _unitOfWork.Brands.CreateAsync(new()
        {
            Title = model.Title,
            Description = model.Description,
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

    public async Task<Response<BrandGetByIdResponse>> GetByIdAsync(int id)
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
                Title = obj.Title,
                Description = obj.Description,
                ImagePath = obj.ImagePath
            }
        };
    }

    public async Task<Response<BrandUpdateRequest>> UpdateAsync(int id, BrandUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Brand]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        obj.Title = model.Title;
        obj.Description = model.Description;
        obj.ImagePath = model.ImagePath;

        _unitOfWork.Brands.Update(obj);
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
                _sharLocalizer[Localization.Shared.Brand]);

            return new Response<string>()
            {
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Brands.Update(obj);

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
                _sharLocalizer[Localization.Shared.Brand]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        _unitOfWork.Brands.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Deleted],
            IsSuccess = true
        };
    }

    #region Private methods
    private async Task<Brand?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Brands.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}