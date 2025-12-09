using ErpManagement.Domain.DTOs.Request.Shared;
using ErpManagement.Domain.DTOs.Response.Shared;
using ErpManagement.Services.IServices.Products;
namespace ErpManagement.Services.Services.Products;

public class ProductTypeService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                                IHubContext<BroadcastHub, IHubClient> hubContext) : IProductTypeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.ProductTypes.GetSpecificSelectAsync(x => x.IsActive,
            select: x => new SelectListResponse
            {
                Id = x.Id,
                Name = x.Title
            }, orderBy: x =>
            x.OrderByDescending(x => x.Id));

        if (!result.Any())
        {
            string message = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Data = Enumerable.Empty<SelectListResponse>(),
                Message = message,
                Error = message
            };
        }

        return new()
        {
            Data = result,
            IsSuccess = true
        };
    }

    public async Task<Response<ProductTypeGetAllResponse>> GetAllAsync(RequestLangEnum lang, ProductTypeGetAllFiltrationsRequest model)
    {
        var result = new ProductTypeGetAllResponse
        {
            TotalRecords = await _unitOfWork.ProductTypes.CountAsync(),

            Items = (await _unitOfWork.ProductTypes.GetSpecificSelectAsync(null,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new ProductTypePaginatedData
            {
                Id = x.Id,
                Name = x.Title,
                IsActive = x.IsActive
            }, orderBy: x =>
            x.OrderByDescending(x => x.Id))).ToList()
        };

        if (result.TotalRecords is 0)
        {
            string message = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Data = new()
                {
                    Items = new List<ProductTypePaginatedData>()
                },
                Message = message,
                Error = message
            };
        }

        return new()
        {
            Data = result,
            IsSuccess = true
        };
    }

    public async Task<Response<ProductTypeCreateRequest>> CreateAsync(ProductTypeCreateRequest model)
    {
        bool isExist = await _unitOfWork.ProductTypes
            .ExistAsync(x => x.Title.Trim().ToLower() == model.Title.Trim().ToLower());

        if (isExist)
        {
            string message = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer["ProductType"]);

            return new()
            {
                Message = message,
                Error = message
            };
        }

        await _unitOfWork.ProductTypes.CreateAsync(new()
        {
            ParentId = model.ParentId,
            Title = model.Title,
            Type = model.Type,
            Description = model.Description
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

    public async Task<Response<ProductTypeGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null)
        {
            string message = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Message = message,
                Error = message
            };
        }

        return new()
        {
            IsSuccess = true,
            Data = new()
            {
                Id = id,
                ParentId = obj.ParentId,
                Title = obj.Title,
                Type = obj.Type,
                Description = obj.Description,
                IsActive = obj.IsActive
            }
        };
    }

    public async Task<Response<ProductTypeUpdateRequest>> UpdateAsync(int id, ProductTypeUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer["ProductType"]);

            return new()
            {
                Message = message,
                Error = message
            };
        }

        obj.ParentId = model.ParentId;
        obj.Title = model.Title;
        obj.Type = model.Type;
        obj.Description = model.Description;

        _unitOfWork.ProductTypes.Update(obj);
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
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer["ProductType"]);

            return new Response<string>()
            {
                Error = message,
                Message = message
            };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.ProductTypes.Update(obj);

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
            string message = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer["ProductType"]);

            return new()
            {
                Message = message,
                Error = message
            };
        }

        _unitOfWork.ProductTypes.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Deleted],
            IsSuccess = true
        };
    }

    #region Private methods
    private async Task<ProductType?> GetObjByIdAsync(int id) =>
        await _unitOfWork.ProductTypes.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}