using System;
using System.Linq.Expressions;
using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Response.Transaction;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class StockAdjustmentService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                                  IHubContext<BroadcastHub, IHubClient> hubContext) : IStockAdjustmentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    private async Task<StockAdjustment?> GetObjByIdAsync(int id) =>
        await _unitOfWork.StockAdjustments.GetFirstOrDefaultAsync(x => x.Id == id,
            includeProperties: "Warehouse,Items,Items.Product");

    #region Inventory helper

    private async Task AdjustInventoryAsync(
        int warehouseId,
        int productId,
        decimal quantityDelta,
        string direction,
        DateTime movementDate,
        int referenceId,
        string? note)
    {
        var wp = await _unitOfWork.WarehouseProducts
            .GetFirstOrDefaultAsync(x => x.WarehouseId == warehouseId && x.ProductId == productId);

        if (wp is null)
        {
            wp = new WarehouseProduct
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                Quantity = 0,
                IsActive = true
            };

            await _unitOfWork.WarehouseProducts.CreateAsync(wp);
        }

        wp.Quantity += quantityDelta;

        // Clamp to zero (MVP safety)
        if (wp.Quantity < 0) wp.Quantity = 0;

        _unitOfWork.WarehouseProducts.Update(wp);

        var movement = new StockMovement
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            MovementType = "StockAdjustment",
            Direction = direction,
            Quantity = Math.Abs(quantityDelta),
            BalanceAfter = wp.Quantity,
            MovementDate = movementDate,
            ReferenceType = "StockAdjustment",
            ReferenceId = referenceId,
            Note = note,
            IsActive = true
        };

        await _unitOfWork.StockMovements.CreateAsync(movement);
    }

    #endregion

    public async Task<Response<StockAdjustmentGetAllResponse>> GetAllAsync(RequestLangEnum lang, StockAdjustmentGetAllFiltrationsForStockAdjustmentsRequest model)
    {
        Expression<Func<StockAdjustment, bool>>? filter = x => true;

        if (model.WarehouseId.HasValue) filter = filter.AndAlso(x => x.WarehouseId == model.WarehouseId.Value);
        if (model.IsActive.HasValue) filter = filter.AndAlso(x => x.IsActive == model.IsActive.Value);
        if (model.FromDate.HasValue) filter = filter.AndAlso(x => x.AdjustmentDate >= model.FromDate.Value.Date);
        if (model.ToDate.HasValue) filter = filter.AndAlso(x => x.AdjustmentDate <= model.ToDate.Value.Date);
        if (model.ProductId.HasValue) filter = filter.AndAlso(x => x.Items.Any(i => i.ProductId == model.ProductId.Value));

        var total = await _unitOfWork.StockAdjustments.CountAsync(filter);

        var items = (await _unitOfWork.StockAdjustments.GetSpecificSelectAsync(
            filter,
            select: x => new PaginatedStockAdjustmentsData
            {
                Id = x.Id,
                AdjustmentCode = x.AdjustmentCode,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : null,
                AdjustmentDate = x.AdjustmentDate,
                TotalQuantity = x.TotalQuantity,
                IsActive = x.IsActive
            },
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            includeProperties: "Warehouse",
            orderBy: q => q.OrderByDescending(z => z.Id))).ToList();

        if (!items.Any())
        {
            string msg = _sharLocalizer[Localization.NotFoundData];
            return new()
            {
                Data = new StockAdjustmentGetAllResponse
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
            Data = new StockAdjustmentGetAllResponse
            {
                Items = items,
                TotalRecords = total
            },
            IsSuccess = true
        };
    }

    public async Task<Response<StockAdjustmentGetByIdResponse>> GetByIdAsync(int id)
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

        var dto = _mapper.Map<StockAdjustmentGetByIdResponse>(obj);

        if (obj.Items is not null)
        {
            dto.Items = obj.Items.Select(i => new StockAdjustmentItemGetByAdjustmentResponse
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Remark = i.Remark,
                ProductName = i.Product != null ? i.Product.Title : null
            }).ToList();
        }

        dto.WarehouseName = obj.Warehouse?.Name;

        return new() { Data = dto, IsSuccess = true };
    }

    public async Task<Response<StockAdjustmentCreateRequest>> CreateAsync(StockAdjustmentCreateRequest model)
    {
        // Validate warehouse
        bool warehouseExist = await _unitOfWork.Warehouses.ExistAsync(x => x.Id == model.WarehouseId && x.IsActive);
        if (!warehouseExist)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.Organization.Warehouse]");
            return new() { Error = msg, Message = msg };
        }

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

        // Disallow zero-quantity lines
        foreach (var it in model.Items)
        {
            if (it.Quantity == 0)
            {
                string msg = _sharLocalizer[Localization.Error];
                return new() { Error = msg, Message = msg };
            }

            // If negative (OUT), ensure not dropping below zero (MVP check)
            if (it.Quantity < 0)
            {
                var wp = await _unitOfWork.WarehouseProducts.GetFirstOrDefaultAsync(x => x.WarehouseId == model.WarehouseId && x.ProductId == it.ProductId);
                var available = wp?.Quantity ?? 0m;
                if (available + it.Quantity < 0) // it.Quantity is negative
                {
                    string msg = _sharLocalizer[Localization.Error];
                    return new() { Error = msg, Message = msg };
                }
            }
        }

        decimal totalQty = model.Items.Sum(x => Math.Abs(x.Quantity));

        var adjustment = new StockAdjustment
        {
            WarehouseId = model.WarehouseId,
            AdjustmentCode = model.AdjustmentCode ?? string.Empty,
            AdjustmentDate = model.AdjustmentDate,
            Reason = model.Reason,
            Note = model.Note,
            TotalQuantity = totalQty,
            IsActive = true
        };

        await _unitOfWork.StockAdjustments.CreateAsync(adjustment);
        await _unitOfWork.CompleteAsync(); // get Id

        var items = model.Items.Select(i => new StockAdjustmentItem
        {
            StockAdjustmentId = adjustment.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Remark = i.Remark
        }).ToList();

        if (items.Any())
        {
            await _unitOfWork.StockAdjustmentItems.CreateRangeAsync(items);
            await _unitOfWork.CompleteAsync();
        }

        if (items.Any())
        {
            foreach (var it in items)
            {
                var direction = it.Quantity > 0 ? "In" : "Out";
                await AdjustInventoryAsync(
                    warehouseId: adjustment.WarehouseId,
                    productId: it.ProductId,
                    quantityDelta: it.Quantity,
                    direction: direction,
                    movementDate: adjustment.AdjustmentDate,
                    referenceId: adjustment.Id,
                    note: adjustment.Note
                );
            }

            await _unitOfWork.CompleteAsync();
        }

        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Done] };
    }

    public async Task<Response<StockAdjustmentUpdateRequest>> UpdateAsync(int id, StockAdjustmentUpdateRequest model)
    {
        if (id != model.Id) return new() { Message = "_sharLocalizer[Localization.InvalidId]" };

        var existing = await GetObjByIdAsync(id);
        if (existing is null) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        bool warehouseExist = await _unitOfWork.Warehouses.ExistAsync(x => x.Id == model.WarehouseId && x.IsActive);
        if (!warehouseExist) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        if (!model.Items.Any()) return new() { Message = _sharLocalizer[Localization.Error] };

        var productIds = model.Items.Select(i => i.ProductId).Distinct().ToList();
        var existingProductsCount = await _unitOfWork.Product.CountAsync(x => productIds.Contains(x.Id));
        if (existingProductsCount != productIds.Count) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        // MVP: do not rollback/reapply inventory on update (TODO)
        await _unitOfWork.StockAdjustmentItems.ExecuteDeleteAsync(x => x.StockAdjustmentId == existing.Id);

        existing.WarehouseId = model.WarehouseId;
        existing.AdjustmentDate = model.AdjustmentDate;
        existing.AdjustmentCode = model.AdjustmentCode ?? existing.AdjustmentCode;
        existing.Reason = model.Reason;
        existing.Note = model.Note;

        var items = model.Items.Select(i => new StockAdjustmentItem
        {
            StockAdjustmentId = existing.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Remark = i.Remark
        }).ToList();

        existing.TotalQuantity = items.Sum(x => Math.Abs(x.Quantity));

        _unitOfWork.StockAdjustments.Update(existing);

        if (items.Any())
            await _unitOfWork.StockAdjustmentItems.CreateRangeAsync(items);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Updated] };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await _unitOfWork.StockAdjustments.GetByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        obj.IsActive = !obj.IsActive;
        _unitOfWork.StockAdjustments.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = obj.IsActive ? _sharLocalizer[Localization.Activated] : _sharLocalizer[Localization.DeActivated] };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        // TODO: implement inventory rollback if required in future.

        _unitOfWork.StockAdjustments.Delete(obj);

        // Remove items
        await _unitOfWork.StockAdjustmentItems.ExecuteDeleteAsync(x => x.StockAdjustmentId == id);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = _sharLocalizer[Localization.Deleted] };
    }
}

