// ErpManagement.Services.Services.Transactions\SaleInvoiceService.cs
using System.Linq.Expressions;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class SaleInvoiceService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer) : ISaleInvoiceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;

    public async Task<Response<SaleInvoiceGetAllResponse>> GetAllAsync(RequestLangEnum lang, SaleInvoiceGetAllFiltrationsForSaleInvoicesRequest model)
    {
        Expression<Func<Sale, bool>> filter = x =>
            (!model.CustomerId.HasValue || x.CustomerId == model.CustomerId.Value) &&
            (!model.WarehouseId.HasValue || (x.WarehouseId.HasValue && x.WarehouseId.Value == model.WarehouseId.Value)) &&
            (!model.BillerId.HasValue || x.BillerId == model.BillerId.Value) &&
            (string.IsNullOrEmpty(model.SaleCode) || x.SaleCode.Contains(model.SaleCode)) &&
            (string.IsNullOrEmpty(model.PaymentStatus) || x.PaymentStatus == model.PaymentStatus) &&
            (!model.FromDate.HasValue || x.SaleDate >= model.FromDate.Value) &&
            (!model.ToDate.HasValue || x.SaleDate <= model.ToDate.Value) &&
            (!model.IsActive.HasValue || x.IsActive == model.IsActive.Value);

        var total = await _unitOfWork.Sales.CountAsync(filter);

        var items = (await _unitOfWork.Sales.GetSpecificSelectAsync(
            filter,
            select: x => new PaginatedSaleInvoicesData
            {
                Id = x.Id,
                InvoiceCode = x.SaleCode,
                SaleDate = x.SaleDate,
                CustomerName = x.Customer != null ? x.Customer.FirstName : null,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : null,
                BillerName = x.Biller != null ? x.Biller.FirstName : null,
                TotalAmount = x.TotalAmount,
                SaleStatus = x.SaleStatus,
                PaymentStatus = x.PaymentStatus,
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
                Data = new SaleInvoiceGetAllResponse { Items = [], TotalRecords = 0 },
                Message = msg,
                Error = msg
            };
        }

        return new()
        {
            Data = new SaleInvoiceGetAllResponse { Items = items, TotalRecords = total },
            IsSuccess = true
        };
    }

    public async Task<Response<SaleInvoiceGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await _unitOfWork.Sales.GetFirstOrDefaultAsync(x => x.Id == id,
            includeProperties: "Customer,Warehouse,Biller,Items,Items.Product");

        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Data = null!, Message = msg, Error = msg };
        }

        var dto = new SaleInvoiceGetByIdResponse
        {
            Id = obj.Id,
            SaleCode = obj.SaleCode,
            SaleDate = obj.SaleDate,
            CustomerName = obj.Customer != null ? $"{obj.Customer.FirstName} {obj.Customer.LastName}".Trim() : null,
            CustomerPhone = obj.Customer?.Phone,
            CustomerAddress = obj.Customer?.Address,
            WarehouseName = obj.Warehouse?.Name,
            BillerName = obj.Biller != null ? $"{obj.Biller.FirstName} {obj.Biller.LastName}".Trim() : null,
            Amount = obj.Amount,
            TaxAmount = obj.TaxAmount,
            Discount = obj.Discount,
            ShippingAmount = obj.ShippingAmount,
            TotalAmount = obj.TotalAmount,
            SaleStatus = obj.SaleStatus,
            PaymentStatus = obj.PaymentStatus,
            Note = obj.Note,
            Items = obj.Items?.Select(i => new SaleInvoiceItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Title,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Amount = i.Amount,
                TaxAmount = i.TaxAmount,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }).ToList() ?? new List<SaleInvoiceItemDto>()
        };

        return new() { Data = dto, IsSuccess = true };
    }
}