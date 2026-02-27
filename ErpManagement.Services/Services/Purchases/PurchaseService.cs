using ErpManagement.Domain.DTOs.Request.Purchases;
using ErpManagement.Domain.DTOs.Response.Purchases;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class PurchaseService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                            IHubContext<BroadcastHub, IHubClient> hubContext) : IPurchaseService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    #region Private Methods

    private async Task<Purchase?> GetObjByIdAsync(int id)
    {
        return await _unitOfWork.Purchases
            .GetFirstOrDefaultAsync(x => x.Id == id,
                includeProperties: "Supplier,Warehouse,Items,Items.Product");
    }

    /// <summary>
    /// Adjust inventory and create a stock movement for a purchase-related change.
    /// quantityDelta:
    ///   > 0  => stock IN (e.g. new purchase, apply on update)
    ///   < 0  => stock OUT (e.g. rollback on update, delete)
    /// unitCost: unit purchase cost, used to recompute AverageCost on stock IN movements.
    /// </summary>
    private async Task AdjustInventoryForPurchaseAsync(
        int warehouseId,
        int productId,
        decimal quantityDelta,
        decimal unitCost,
        string movementType,
        string direction,
        DateTime movementDate,
        int referenceId,
        string? note)
    {
        var wp = await _unitOfWork.WarehouseProducts
            .GetFirstOrDefaultAsync(x =>
                x.WarehouseId == warehouseId &&
                x.ProductId == productId);

        if (wp is null)
        {
            wp = new WarehouseProduct
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                Quantity = 0,
                AverageCost = 0,
                IsActive = true
            };

            await _unitOfWork.WarehouseProducts.CreateAsync(wp);
        }

        decimal oldQty = wp.Quantity;
        decimal oldAvg = wp.AverageCost;
        wp.Quantity += quantityDelta;

        // Recompute Weighted Average Cost only on stock IN
        if (quantityDelta > 0 && wp.Quantity > 0)
        {
            wp.AverageCost = oldQty > 0
                ? ((oldQty * oldAvg) + (quantityDelta * unitCost)) / wp.Quantity
                : unitCost;
        }

        _unitOfWork.WarehouseProducts.Update(wp);

        var movement = new StockMovement
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            MovementType = movementType,      // e.g. "Purchase", "PurchaseUpdateRollback", "PurchaseDelete"
            Direction = direction,            // "In" or "Out"
            Quantity = Math.Abs(quantityDelta),
            BalanceAfter = wp.Quantity,       // 🔥 important for reporting
            MovementDate = movementDate,
            ReferenceType = "Purchase",
            ReferenceId = referenceId,
            Note = note,
            IsActive = true
        };

        await _unitOfWork.StockMovements.CreateAsync(movement);
    }

    #endregion

    public async Task<Response<PurchaseGetAllResponse>> GetAllAsync(RequestLangEnum lang, PurchaseGetAllFiltrationsForPurchasesRequest model)
    {
        var total = await _unitOfWork.Purchases.CountAsync();

        var items = (await _unitOfWork.Purchases.GetSpecificSelectAsync(
            null,
            select: x => new PaginatedPurchasesData
            {
                Id = x.Id,
                PurchaseCode = x.PurchaseCode,
                SupplierName = x.Supplier.FirstName,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : null,
                PurchaseDate = x.PurchaseDate,
                TotalAmount = x.TotalAmount,
                PurchaseStatus = x.PurchaseStatus,
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
                Data = new PurchaseGetAllResponse
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
            Data = new PurchaseGetAllResponse
            {
                Items = items,
                TotalRecords = total
            },
            IsSuccess = true
        };
    }

    public async Task<Response<PurchaseGetByIdResponse>> GetByIdAsync(int id)
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

        var dto = _mapper.Map<PurchaseGetByIdResponse>(obj);
        return new()
        {
            Data = dto,
            IsSuccess = true
        };
    }

    public async Task<Response<PurchaseCreateRequest>> CreateAsync(PurchaseCreateRequest model)
    {
        #region Validations

        // Supplier must exist and be active
        bool supplierExist = await _unitOfWork.Supplier
            .ExistAsync(x => x.Id == model.SupplierId && x.IsActive);
        if (!supplierExist)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Shared.Supplier]);
            return new()
            {
                Error = msg,
                Message = msg
            };
        }

        // Must have at least one item
        if (!model.Items.Any())
        {
            string msg = _sharLocalizer[Localization.Error];
            return new()
            {
                Error = msg,
                Message = msg
            };
        }

        // Validate products exist
        var productIds = model.Items.Select(i => i.ProductId).Distinct().ToList();
        var existingProductsCount = await _unitOfWork.Product
            .CountAsync(x => productIds.Contains(x.Id));

        if (existingProductsCount != productIds.Count)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new()
            {
                Error = msg,
                Message = msg
            };
        }

        // Validate item quantities and prices
        foreach (var it in model.Items)
        {
            if (it.Quantity <= 0 || it.UnitPrice < 0)
            {
                string msg = _sharLocalizer[Localization.Error];
                return new()
                {
                    Error = msg,
                    Message = msg
                };
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

        #region Create Purchase

        var purchase = new Purchase
        {
            SupplierId = model.SupplierId,
            WarehouseId = model.WarehouseId,
            PurchaseCode = model.PurchaseCode ?? string.Empty,
            PurchaseDate = model.PurchaseDate,
            ShippingAmount = model.ShippingAmount,
            Amount = amount,
            TaxAmount = taxAmount,
            Discount = discount,
            TotalAmount = total,
            Note = model.Note,
            PurchaseStatus = "Pending",
            IsActive = true
        };

        await _unitOfWork.Purchases.CreateAsync(purchase);
        await _unitOfWork.CompleteAsync(); // ensure Id is generated

        #endregion

        #region Create Purchase Items

        var items = model.Items.Select(i => new PurchaseItem
        {
            PurchaseId = purchase.Id,
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
            await _unitOfWork.PurchaseItems.CreateRangeAsync(items);
            await _unitOfWork.CompleteAsync();
        }

        #endregion

        #region Stock Movements & WarehouseProduct (Inventory)

        if (items.Any() && purchase.WarehouseId.HasValue)
        {
            var warehouseId = purchase.WarehouseId.Value;

            foreach (var it in items)
            {
                await AdjustInventoryForPurchaseAsync(
                    warehouseId: warehouseId,
                    productId: it.ProductId,
                    quantityDelta: it.Quantity,               // stock IN
                    unitCost: it.UnitPrice,
                    movementType: "Purchase",
                    direction: "In",
                    movementDate: purchase.PurchaseDate,
                    referenceId: purchase.Id,
                    note: purchase.Note
                );
            }

            await _unitOfWork.CompleteAsync();
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

    public async Task<Response<PurchaseUpdateRequest>> UpdateAsync(int id, PurchaseUpdateRequest model)
    {
        var existing = await GetObjByIdAsync(id);
        if (existing is null || id != model.Id)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new()
            {
                Error = msg,
                Message = msg
            };
        }

        #region Inventory rollback for old items

        if (existing.WarehouseId.HasValue && existing.Items is not null && existing.Items.Any())
        {
            var warehouseId = existing.WarehouseId.Value;

            foreach (var oldItem in existing.Items)
            {
                await AdjustInventoryForPurchaseAsync(
                    warehouseId: warehouseId,
                    productId: oldItem.ProductId,
                    quantityDelta: -oldItem.Quantity,             // stock OUT (rollback)
                    unitCost: 0,                                  // not used for OUT
                    movementType: "PurchaseUpdateRollback",
                    direction: "Out",
                    movementDate: DateTime.UtcNow,
                    referenceId: existing.Id,
                    note: existing.Note
                );
            }
        }

        #endregion

        #region Remove old items

        await _unitOfWork.PurchaseItems.ExecuteDeleteAsync(x => x.PurchaseId == existing.Id);

        #endregion

        #region Update header + rebuild items

        existing.SupplierId = model.SupplierId;
        existing.WarehouseId = model.WarehouseId;
        existing.PurchaseDate = model.PurchaseDate;
        existing.PurchaseCode = model.PurchaseCode ?? existing.PurchaseCode;
        existing.ShippingAmount = model.ShippingAmount;
        existing.Note = model.Note;

        decimal amount = 0M;
        decimal taxAmount = 0M;
        decimal discount = 0M;

        var items = model.Items.Select(i => new PurchaseItem
        {
            PurchaseId = existing.Id,
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

        _unitOfWork.Purchases.Update(existing);
        await _unitOfWork.PurchaseItems.CreateRangeAsync(items);

        #endregion

        #region Apply inventory for new items

        if (existing.WarehouseId.HasValue && items.Any())
        {
            var warehouseId = existing.WarehouseId.Value;

            foreach (var it in items)
            {
                await AdjustInventoryForPurchaseAsync(
                    warehouseId: warehouseId,
                    productId: it.ProductId,
                    quantityDelta: it.Quantity,              // stock IN (new state)
                    unitCost: it.UnitPrice,
                    movementType: "PurchaseUpdateApply",
                    direction: "In",
                    movementDate: existing.PurchaseDate,
                    referenceId: existing.Id,
                    note: existing.Note
                );
            }
        }

        #endregion

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
        var obj = await _unitOfWork.Purchases.GetByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new()
            {
                Error = msg,
                Message = msg
            };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Purchases.Update(obj);
        await _unitOfWork.CompleteAsync();

        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            IsSuccess = true,
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
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new()
            {
                Error = msg,
                Message = msg
            };
        }

        #region Inventory rollback on delete

        if (obj.WarehouseId.HasValue && obj.Items is not null && obj.Items.Any())
        {
            var warehouseId = obj.WarehouseId.Value;

            foreach (var item in obj.Items)
            {
                await AdjustInventoryForPurchaseAsync(
                    warehouseId: warehouseId,
                    productId: item.ProductId,
                    quantityDelta: -item.Quantity,          // stock OUT
                    unitCost: 0,                            // not used for OUT
                    movementType: "PurchaseDelete",
                    direction: "Out",
                    movementDate: DateTime.UtcNow,
                    referenceId: obj.Id,
                    note: obj.Note
                );
            }
        }

        #endregion

        _unitOfWork.Purchases.Delete(obj);

        // Remove items
        await _unitOfWork.PurchaseItems.ExecuteDeleteAsync(x => x.PurchaseId == id);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            IsSuccess = true,
            Message = _sharLocalizer[Localization.Deleted]
        };
    }
}
