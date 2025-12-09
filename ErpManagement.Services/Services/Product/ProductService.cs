using ErpManagement.Domain.DTOs.Request.Products;
using ErpManagement.Domain.DTOs.Response.Products;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Services.IServices.Products;
namespace ErpManagement.Services.Services.Products;

public class ProductService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                            IHubContext<BroadcastHub, IHubClient> hubContext) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    public async Task<Response<IEnumerable<SelectListResponse>>> ListAsync(RequestLangEnum lang)
    {
        var result = await _unitOfWork.Product.GetSpecificSelectAsync(x => x.IsActive,
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

    public async Task<Response<ProductGetAllResponse>> GetAllAsync(RequestLangEnum lang, ProductGetAllFiltrationsRequest model)
    {
        var result = new ProductGetAllResponse
        {
            TotalRecords = await _unitOfWork.Product.CountAsync(),

            Items = (await _unitOfWork.Product.GetSpecificSelectAsync(null,
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            select: x => new ProductPaginatedData
            {
                Id = x.Id,
                Name = x.Title,
                ProductCode = x.ProductCode,
                Price = x.Price,
                Quantity = x.Quantity,
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

    public async Task<Response<ProductCreateRequest>> CreateAsync(ProductCreateRequest model)
    {
        bool isExist = await _unitOfWork.Product
            .ExistAsync(x => x.ProductCode.Trim().ToLower() == model.ProductCode.Trim().ToLower());

        if (isExist)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Shared.Product]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        await _unitOfWork.Product.CreateAsync(new()
        {
            ProductCode = model.ProductCode,
            Title = model.Title,
            Description = model.Description,
            CategoryId = model.CategoryId,
            SupplierId = model.SupplierId,
            BrandId = model.BrandId,
            TypeId = model.TypeId,
            UnitId = model.UnitId,
            TaxId = model.TaxId,
            Price = model.Price,
            Cost = model.Cost,
            Tax = model.Tax,
            Discount = model.Discount,
            Quantity = model.Quantity,
            AlertQuantity = model.AlertQuantity,
            ImagePath = model.ImagePath,
            Barcode = model.Barcode,
            SKU = model.SKU,
            IsFeatured = model.IsFeatured,
            IsExpired = model.IsExpired,
            IsPromoSale = model.IsPromoSale,
            ExpiryDate = model.ExpiryDate,
            ManufactureDate = model.ManufactureDate,
            ColorVariantIds = model.ColorVariantIds,
            SizeVariantIds = model.SizeVariantIds
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

    public async Task<Response<ProductGetByIdResponse>> GetByIdAsync(int id)
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
                ProductCode = obj.ProductCode,
                Title = obj.Title,
                Description = obj.Description,
                CategoryId = obj.CategoryId,
                SupplierId = obj.SupplierId,
                BrandId = obj.BrandId,
                TypeId = obj.TypeId,
                UnitId = obj.UnitId,
                TaxId = obj.TaxId,
                Price = obj.Price,
                Cost = obj.Cost,
                Tax = obj.Tax,
                Discount = obj.Discount,
                Quantity = obj.Quantity,
                AlertQuantity = obj.AlertQuantity,
                ImagePath = obj.ImagePath,
                Barcode = obj.Barcode,
                SKU = obj.SKU,
                IsFeatured = obj.IsFeatured,
                IsExpired = obj.IsExpired,
                IsPromoSale = obj.IsPromoSale,
                ExpiryDate = obj.ExpiryDate,
                ManufactureDate = obj.ManufactureDate,
                IsActive = obj.IsActive,
                ColorVariantIds = obj.ColorVariantIds,
                SizeVariantIds = obj.SizeVariantIds
            }
        };
    }

    public async Task<Response<ProductUpdateRequest>> UpdateAsync(int id, ProductUpdateRequest model)
    {
        var obj = await GetObjByIdAsync(id);

        if (obj is null || id != model.Id)
        {
            string resultIsSuccess = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Product]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        obj.ProductCode = model.ProductCode;
        obj.Title = model.Title;
        obj.Description = model.Description;
        obj.CategoryId = model.CategoryId;
        obj.SupplierId = model.SupplierId;
        obj.BrandId = model.BrandId;
        obj.TypeId = model.TypeId;
        obj.UnitId = model.UnitId;
        obj.TaxId = model.TaxId;
        obj.Price = model.Price;
        obj.Cost = model.Cost;
        obj.Tax = model.Tax;
        obj.Discount = model.Discount;
        obj.Quantity = model.Quantity;
        obj.AlertQuantity = model.AlertQuantity;
        obj.ImagePath = model.ImagePath;
        obj.Barcode = model.Barcode;
        obj.SKU = model.SKU;
        obj.IsFeatured = model.IsFeatured;
        obj.IsExpired = model.IsExpired;
        obj.IsPromoSale = model.IsPromoSale;
        obj.ExpiryDate = model.ExpiryDate;
        obj.ManufactureDate = model.ManufactureDate;
        obj.ColorVariantIds = model.ColorVariantIds;
        obj.SizeVariantIds = model.SizeVariantIds;

        _unitOfWork.Product.Update(obj);
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
                _sharLocalizer[Localization.Shared.Product]);

            return new Response<string>()
            {
                Error = resultIsSuccess,
                Message = resultIsSuccess
            };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Product.Update(obj);

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
                _sharLocalizer[Localization.Shared.Product]);

            return new()
            {
                Message = resultIsSuccess,
                Error = resultIsSuccess
            };
        }

        _unitOfWork.Product.Delete(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Message = _sharLocalizer[Localization.Deleted],
            IsSuccess = true
        };
    }

    #region Private methods
    private async Task<Product?> GetObjByIdAsync(int id) =>
        await _unitOfWork.Product.GetFirstOrDefaultAsync(x => x.Id == id);
    #endregion
}