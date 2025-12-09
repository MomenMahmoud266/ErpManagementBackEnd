using ErpManagement.Domain.DTOs.Request.People.Customer;
using ErpManagement.Domain.DTOs.Response.People.Customer;
using ErpManagement.Domain.Models.People;
using ErpManagement.Services.IServices.Shared;

namespace ErpManagement.Services.Services.People;

public class CustomerService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                            IHubContext<BroadcastHub, IHubClient> hubContext) : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Customer.GetSpecificSelectAsync(x => x.IsActive,
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

    public async Task<Response<CustomerGetAllResponse>> GetAllAsync(RequestLangEnum lang, CustomerGetAllFiltrationsRequest model)
    {
        var result = new CustomerGetAllResponse
        {
            TotalRecords = await _unitOfWork.Customer.CountAsync(),

            Items = (await _unitOfWork.Customer.GetSpecificSelectAsync(null,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new CustomerPaginatedData
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

    public async Task<Response<CustomerCreateRequest>> CreateAsync(CustomerCreateRequest model)
    {
        bool isExist = await _unitOfWork.Customer
            .ExistAsync(x => x.CustomerCode.Trim().ToLower() == model.CustomerCode.Trim().ToLower());

        if (isExist)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Shared.Customer]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        await _unitOfWork.Customer.CreateAsync(new()
        {
            CountryId = model.CountryId,
            CategoryId = model.CategoryId,
            CustomerCode = model.CustomerCode,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Email = model.Email,
            City = model.City,
            Address = model.Address,
            ZipCode = model.ZipCode,
            RewardPoints = model.RewardPoints,
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

    public async Task<Response<CustomerGetByIdResponse>> GetByIdAsync(int id)
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
                CategoryId = obj.CategoryId,
                CustomerCode = obj.CustomerCode,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Phone = obj.Phone,
                Email = obj.Email,
                City = obj.City,
                Address = obj.Address,
                ZipCode = obj.ZipCode,
                RewardPoints = obj.RewardPoints,
                ImagePath = obj.ImagePath,
                IsActive = obj.IsActive
            }
        };
    }

    public async Task<Response<CustomerUpdateRequest>> UpdateAsync(int id, CustomerUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Customer]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        obj.CountryId = model.CountryId;
        obj.CategoryId = model.CategoryId;
        obj.CustomerCode = model.CustomerCode;
        obj.FirstName = model.FirstName;
        obj.LastName = model.LastName;
        obj.Phone = model.Phone;
        obj.Email = model.Email;
        obj.City = model.City;
        obj.Address = model.Address;
        obj.ZipCode = model.ZipCode;
        obj.RewardPoints = model.RewardPoints;
        obj.ImagePath = model.ImagePath;

        _unitOfWork.Customer.Update(obj);
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
                _sharLocalizer[Localization.Shared.Customer]);

            return new Response<string>()
            {
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Customer.Update(obj);

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
                _sharLocalizer[Localization.Shared.Customer]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        _unitOfWork.Customer.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Deleted],
            IsSuccess = true
        };
    }

    #region Private methods
    private async Task<Customer?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Customer.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}