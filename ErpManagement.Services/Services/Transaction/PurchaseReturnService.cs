// ErpManagement.Services\Services\Transactions\PurchaseReturnService.cs
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class PurchaseReturnService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                                 IHubContext<BroadcastHub, IHubClient> hubContext) : IPurchaseReturnService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    private async Task<PurchaseReturn?> GetObjByIdAsync(int id)
    {
        return await _unitOfWork.PurchaseReturns
            .GetFirstOrDefaultAsync(x => x.Id == id,
                includeProperties: "Supplier,Warehouse,Purchase,Items,Items.Product");
    }

    public async Task<Response<PurchaseReturnGetAllResponse>> GetAllAsync(RequestLangEnum lang, PurchaseReturnGetAllFiltrationsForPurchaseReturnsRequest model)
    {
        var total = await _unitOfWork.PurchaseReturns.CountAsync();

        var items = (await _unitOfWork.PurchaseReturns.GetSpecificSelectAsync(
            null,
            select: x => new PaginatedPurchaseReturnsData
            {
                Id = x.Id,
                ReturnCode = x.ReturnCode,
                PurchaseCode = x.Purchase != null ? x.Purchase.PurchaseCode : null,
                SupplierName = x.Supplier.FirstName,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : null,
                ReturnDate = x.ReturnDate,
                TotalAmount = x.TotalAmount,
                ReturnStatus = x.ReturnStatus,
                IsActive = x.IsActive
            },
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            orderBy: q => q.OrderByDescending(z => z.Id))).ToList();

        if (!items.Any())
        {
            string msg = _sharLocalizer[Localization.NotFoundData];
            return new()
            {
                Data = new PurchaseReturnGetAllResponse
                {
                    Items = [],
                    TotalRecords = 0
                },
                Message = msg,
                Error = msg
            };
        }

        return new()
        {
            Data = new PurchaseReturnGetAllResponse
            {
                Items = items,
                TotalRecords = total
            },
            IsSuccess = true
        };
    }

    public async Task<Response<PurchaseReturnGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new()
            {
                Data = null!,
                Message = msg,
                Error = msg
            };
        }

        var dto = _mapper.Map<PurchaseReturnGetByIdResponse>(obj);
        return new()
        {
            Data = dto,
            IsSuccess = true
        };
    }

    public async Task<Response<PurchaseReturnCreateRequest>> CreateAsync(PurchaseReturnCreateRequest model)
    {
        #region Validations

        // Purchase must exist (with items)
        var purchase = await _unitOfWork.Purchases.GetFirstOrDefaultAsync(
            x => x.Id == model.PurchaseId,
            includeProperties: "Items"
        );
        if (purchase is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound]," _sharLocalizer[Localization.Shared.Purchase]" ?? "Purchase");
            return new() { Error = msg, Message = msg };
        }

        // Supplier must exist and match
        bool supplierExist = await _unitOfWork.Supplier.ExistAsync(x => x.Id == model.SupplierId && x.IsActive);
        if (!supplierExist || purchase.SupplierId != model.SupplierId)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Supplier]);
            return new() { Error = msg, Message = msg };
        }

        // Items validations
        if (!model.Items.Any())
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Error = msg, Message = msg };
        }

        var productIds = model.Items.Select(i => i.ProductId).Distinct().ToList();
        var existingProductsCount = await _unitOfWork.Product.CountAsync(x => productIds.Contains(x.Id));
        if (existingProductsCount != productIds.Count)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        foreach (var it in model.Items)
        {
            if (it.Quantity <= 0 || it.UnitPrice < 0)
            {
                string msg = _sharLocalizer[Localization.Error];
                return new() { Error = msg, Message = msg };
            }

            var purchaseLine = purchase.Items?.FirstOrDefault(pi => pi.ProductId == it.ProductId);
            if (purchaseLine is null || it.Quantity > purchaseLine.Quantity)
            {
                string msg = _sharLocalizer[Localization.Error];
                return new() { Error = msg, Message = msg };
            }
        }

        #endregion

        #region Totals Calculation

        decimal amount = 0M;
        decimal taxAmount = 0M;
        decimal discount = 0M;

        foreach (var it in model.Items)
        {
            var lineAmount = it.Quantity * it.UnitPrice;
            amount += lineAmount;
            taxAmount += it.TaxAmount ?? 0;
            discount += it.Discount ?? 0;
        }

        decimal total = amount + taxAmount + model.ShippingAmount - discount;

        #endregion

        #region Create PurchaseReturn

        var pr = new PurchaseReturn
        {
            PurchaseId = model.PurchaseId,
            SupplierId = model.SupplierId,
            WarehouseId = model.WarehouseId ?? purchase.WarehouseId,
            ReturnCode = model.ReturnCode ?? string.Empty,
            ReturnDate = model.ReturnDate,
            ShippingAmount = model.ShippingAmount,
            Amount = amount,
            TaxAmount = taxAmount,
            Discount = discount,
            TotalAmount = total,
            ReturnStatus = "Pending",
            Remark = model.Remark,
            ReturnNote = model.ReturnNote,
            IsActive = true
        };

        await _unitOfWork.PurchaseReturns.CreateAsync(pr);
        await _unitOfWork.CompleteAsync(); // ensure Id

        #endregion

        #region Create Items

        var items = model.Items.Select(i => new PurchaseReturnItem
        {
            PurchaseReturnId = pr.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Amount = i.Quantity * i.UnitPrice,
            TaxAmount = i.TaxAmount ?? 0,
            Discount = i.Discount ?? 0,
            TotalAmount = (i.Quantity * i.UnitPrice) + (i.TaxAmount ?? 0) - (i.Discount ?? 0)
        }).ToList();

        if (items.Any())
        {
            await _unitOfWork.PurchaseReturnItems.CreateRangeAsync(items);
            await _unitOfWork.CompleteAsync();
        }

        #endregion

        #region Inventory effect: STOCK OUT (returns decrease stock)

        if (items.Any() && pr.WarehouseId.HasValue)
        {
            var warehouseId = pr.WarehouseId.Value;
            var movements = new List<StockMovement>();

            foreach (var it in items)
            {
                var wp = await _unitOfWork.WarehouseProducts.GetFirstOrDefaultAsync(x =>
                    x.WarehouseId == warehouseId && x.ProductId == it.ProductId);

                if (wp is null)
                {
                    wp = new WarehouseProduct
                    {
                        WarehouseId = warehouseId,
                        ProductId = it.ProductId,
                        Quantity = 0,
                        IsActive = true
                    };
                    await _unitOfWork.WarehouseProducts.CreateAsync(wp);
                }

                wp.Quantity -= it.Quantity;        // stock OUT for a return
                if (wp.Quantity < 0) wp.Quantity = 0;
                _unitOfWork.WarehouseProducts.Update(wp);

                var movement = new StockMovement
                {
                    ProductId = it.ProductId,
                    WarehouseId = warehouseId,
                    MovementType = "PurchaseReturn",
                    Direction = "Out",
                    Quantity = it.Quantity,
                    BalanceAfter = wp.Quantity,
                    MovementDate = pr.ReturnDate,
                    ReferenceType = "PurchaseReturn",
                    ReferenceId = pr.Id,
                    Note = pr.ReturnNote,
                    IsActive = true
                };

                movements.Add(movement);
            }

            if (movements.Any())
            {
                await _unitOfWork.StockMovements.CreateRangeAsync(movements);
                await _unitOfWork.CompleteAsync();
            }
        }

        #endregion

        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Data = model,
            IsSuccess = true,
            Message = _sharLocalizer[Localization.Done]
        };
    }

    public async Task<Response<PurchaseReturnUpdateRequest>> UpdateAsync(int id, PurchaseReturnUpdateRequest model)
    {
        var existing = await GetObjByIdAsync(id);
        if (existing is null || id != model.Id)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        // MVP: Do not adjust inventory for update (TODO: implement rollback/apply if required)
        // Remove old items
        await _unitOfWork.PurchaseReturnItems.ExecuteDeleteAsync(x => x.PurchaseReturnId == existing.Id);

        // Update header
        existing.SupplierId = model.SupplierId;
        existing.WarehouseId = model.WarehouseId;
        existing.PurchaseId = model.PurchaseId;
        existing.ReturnCode = model.ReturnCode ?? existing.ReturnCode;
        existing.ReturnDate = model.ReturnDate;
        existing.ShippingAmount = model.ShippingAmount;
        existing.Remark = model.Remark;
        existing.ReturnNote = model.ReturnNote;

        decimal amount = 0M;
        decimal taxAmount = 0M;
        decimal discount = 0M;

        var items = model.Items.Select(i => new PurchaseReturnItem
        {
            PurchaseReturnId = existing.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Amount = i.Quantity * i.UnitPrice,
            TaxAmount = i.TaxAmount ?? 0,
            Discount = i.Discount ?? 0,
            TotalAmount = (i.Quantity * i.UnitPrice) + (i.TaxAmount ?? 0) - (i.Discount ?? 0)
        }).ToList();

        foreach (var it in items)
        {
            amount += it.Amount;
            taxAmount += it.TaxAmount;
            discount += it.Discount;
        }

        existing.Amount = amount;
        existing.TaxAmount = taxAmount;
        existing.Discount = discount;
        existing.TotalAmount = amount + taxAmount + existing.ShippingAmount - discount;

        _unitOfWork.PurchaseReturns.Update(existing);
        if (items.Any()) await _unitOfWork.PurchaseReturnItems.CreateRangeAsync(items);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Data = model,
            IsSuccess = true,
            Message = _sharLocalizer[Localization.Updated]
        };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await _unitOfWork.PurchaseReturns.GetByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.PurchaseReturns.Update(obj);
        await _unitOfWork.CompleteAsync();

        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            IsSuccess = true,
            Message = obj.IsActive ? _sharLocalizer[Localization.Activated] : _sharLocalizer[Localization.DeActivated]
        };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        // MVP: Do NOT rollback inventory on delete (TODO: implement if needed)
        _unitOfWork.PurchaseReturns.Delete(obj);
        await _unitOfWork.PurchaseReturnItems.ExecuteDeleteAsync(x => x.PurchaseReturnId == id);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = _sharLocalizer[Localization.Deleted] };
    }
}