// ErpManagement.Services.Services.Transactions\PurchaseInvoiceService.cs
using System.Linq.Expressions;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class PurchaseInvoiceService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : IPurchaseInvoiceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<PurchaseInvoiceGetAllResponse>> GetAllAsync(RequestLangEnum lang, PurchaseInvoiceGetAllFiltrationsForPurchaseInvoicesRequest model)
    {
        Expression<Func<Purchase, bool>> filter = x =>
            (!model.SupplierId.HasValue || x.SupplierId == model.SupplierId.Value) &&
            (!model.WarehouseId.HasValue || (x.WarehouseId.HasValue && x.WarehouseId.Value == model.WarehouseId.Value)) &&
            (string.IsNullOrEmpty(model.PurchaseCode) || x.PurchaseCode.Contains(model.PurchaseCode)) &&
            (!model.FromDate.HasValue || x.PurchaseDate >= model.FromDate.Value) &&
            (!model.ToDate.HasValue || x.PurchaseDate <= model.ToDate.Value) &&
            (!model.IsActive.HasValue || x.IsActive == model.IsActive.Value);

        var total = await _unitOfWork.Purchases.CountAsync(filter);

        var items = (await _unitOfWork.Purchases.GetSpecificSelectAsync(
            filter,
            select: x => new PaginatedPurchaseInvoicesData
            {
                Id = x.Id,
                InvoiceCode = x.PurchaseCode,
                PurchaseDate = x.PurchaseDate,
                SupplierName = x.Supplier != null ? x.Supplier.FirstName : null,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : null,
                TotalAmount = x.TotalAmount,
                PurchaseStatus = x.PurchaseStatus,
                IsActive = x.IsActive
            },
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            orderBy: q => q.OrderByDescending(z => z.Id)
        )).ToList();

        if (!items.Any())
        {
            string msg = _sharLocalizer[Localization.NotFoundData];
            return new()
            {
                Data = new PurchaseInvoiceGetAllResponse { Items = [], TotalRecords = 0 },
                Message = msg,
                Error = msg
            };
        }

        return new() { Data = new PurchaseInvoiceGetAllResponse { Items = items, TotalRecords = total }, IsSuccess = true };
    }

    public async Task<Response<PurchaseInvoiceGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await _unitOfWork.Purchases.GetFirstOrDefaultAsync(x => x.Id == id,
            includeProperties: "Supplier,Warehouse,Items,Items.Product");

        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Data = null!, Message = msg, Error = msg };
        }

        var dto = _mapper.Map<PurchaseInvoiceGetByIdResponse>(obj);
        // In case AutoMapper not configured for everything, ensure items mapped:
        if (dto.Items is null && obj.Items != null)
        {
            dto.Items = obj.Items.Select(i => new PurchaseInvoiceItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Title,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Amount = i.Amount,
                TaxAmount = i.TaxAmount,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }).ToList();
        }

        return new() { Data = dto, IsSuccess = true };
    }
}