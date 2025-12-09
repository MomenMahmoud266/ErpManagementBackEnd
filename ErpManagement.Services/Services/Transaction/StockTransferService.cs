using System;
using System.Linq.Expressions;
using ErpManagement.Domain.DTOs.Request.Transaction;
using ErpManagement.Domain.DTOs.Response.Transaction;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class StockTransferService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                                 IHubContext<BroadcastHub, IHubClient> hubContext) : IStockTransferService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    private async Task<StockTransfer?> GetObjByIdAsync(int id) =>
        await _unitOfWork.StockTransfers.GetFirstOrDefaultAsync(x => x.Id == id,
            includeProperties: "FromWarehouse,ToWarehouse,Items,Items.Product");

    #region Inventory helper

    private async Task AdjustInventoryAsync(
        int warehouseId,
        int productId,
        decimal quantityDelta,
        string movementType,
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
            MovementType = movementType,
            Direction = direction,
            Quantity = Math.Abs(quantityDelta),
            BalanceAfter = wp.Quantity,
            MovementDate = movementDate,
            ReferenceType = "StockTransfer",
            ReferenceId = referenceId,
            Note = note,
            IsActive = true
        };

        await _unitOfWork.StockMovements.CreateAsync(movement);
    }

    #endregion

    public async Task<Response<StockTransferGetAllResponse>> GetAllAsync(RequestLangEnum lang, StockTransferGetAllFiltrationsForStockTransfersRequest model)
    {
        Expression<Func<StockTransfer, bool>>? filter = x => true;

        if (model.FromWarehouseId.HasValue) filter = filter.AndAlso(x => x.FromWarehouseId == model.FromWarehouseId.Value);
        if (model.ToWarehouseId.HasValue) filter = filter.AndAlso(x => x.ToWarehouseId == model.ToWarehouseId.Value);
        if (model.IsActive.HasValue) filter = filter.AndAlso(x => x.IsActive == model.IsActive.Value);
        if (model.FromDate.HasValue) filter = filter.AndAlso(x => x.TransferDate >= model.FromDate.Value.Date);
        if (model.ToDate.HasValue) filter = filter.AndAlso(x => x.TransferDate <= model.ToDate.Value.Date);
        if (model.ProductId.HasValue) filter = filter.AndAlso(x => x.Items.Any(i => i.ProductId == model.ProductId.Value));

        var total = await _unitOfWork.StockTransfers.CountAsync(filter);

        var items = (await _unitOfWork.StockTransfers.GetSpecificSelectAsync(
            filter,
            select: x => new PaginatedStockTransfersData
            {
                Id = x.Id,
                TransferCode = x.TransferCode,
                FromWarehouseName = x.FromWarehouse != null ? x.FromWarehouse.Name : null,
                ToWarehouseName = x.ToWarehouse != null ? x.ToWarehouse.Name : null,
                TransferDate = x.TransferDate,
                TotalQuantity = x.TotalQuantity,
                TransferStatus = x.TransferStatus,
                IsActive = x.IsActive
            },
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            includeProperties: "FromWarehouse,ToWarehouse",
            orderBy: q => q.OrderByDescending(z => z.Id))).ToList();

        if (!items.Any())
        {
            string msg = _sharLocalizer[Localization.NotFoundData];
            return new()
            {
                Data = new StockTransferGetAllResponse
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
            Data = new StockTransferGetAllResponse
            {
                Items = items,
                TotalRecords = total
            },
            IsSuccess = true
        };
    }

    public async Task<Response<StockTransferGetByIdResponse>> GetByIdAsync(int id)
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

        var dto = _mapper.Map<StockTransferGetByIdResponse>(obj);

        if (obj.Items is not null)
        {
            dto.Items = obj.Items.Select(i => new StockTransferItemGetByTransferResponse
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Remark = i.Remark,
                ProductName = i.Product != null ? i.Product.Title : null
            }).ToList();
        }

        dto.FromWarehouseName = obj.FromWarehouse?.Name;
        dto.ToWarehouseName = obj.ToWarehouse?.Name;

        return new() { Data = dto, IsSuccess = true };
    }

    public async Task<Response<StockTransferCreateRequest>> CreateAsync(StockTransferCreateRequest model)
    {
        // Validate warehouses
        bool fromExist = await _unitOfWork.Warehouses.ExistAsync(x => x.Id == model.FromWarehouseId && x.IsActive);
        if (!fromExist)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.Organization.Warehouse]");
            return new() { Error = msg, Message = msg };
        }

        bool toExist = await _unitOfWork.Warehouses.ExistAsync(x => x.Id == model.ToWarehouseId && x.IsActive);
        if (!toExist)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.Organization.Warehouse]");
            return new() { Error = msg, Message = msg };
        }

        if (model.FromWarehouseId == model.ToWarehouseId)
        {
            string msg = _sharLocalizer[Localization.Error];
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

        foreach (var it in model.Items)
        {
            if (it.Quantity <= 0)
            {
                string msg = _sharLocalizer[Localization.Error];
                return new() { Error = msg, Message = msg };
            }
        }

        // Optionally check stock availability at source warehouse
        foreach (var it in model.Items)
        {
            var wp = await _unitOfWork.WarehouseProducts.GetFirstOrDefaultAsync(x => x.WarehouseId == model.FromWarehouseId && x.ProductId == it.ProductId);
            var available = wp?.Quantity ?? 0m;
            if (available < it.Quantity)
            {
                string msg = _sharLocalizer[Localization.Error]; // no specific key for insufficient stock in repo; use generic
                return new() { Error = msg, Message = msg };
            }
        }

        decimal totalQty = model.Items.Sum(x => x.Quantity);

        var transfer = new StockTransfer
        {
            FromWarehouseId = model.FromWarehouseId,
            ToWarehouseId = model.ToWarehouseId,
            TransferCode = model.TransferCode ?? string.Empty,
            TransferDate = model.TransferDate,
            Remark = model.Remark,
            TransferNote = model.TransferNote,
            TotalQuantity = totalQty,
            TransferStatus = "Pending",
            IsActive = true
        };

        await _unitOfWork.StockTransfers.CreateAsync(transfer);
        await _unitOfWork.CompleteAsync(); // get Id

        var items = model.Items.Select(i => new StockTransferItem
        {
            StockTransferId = transfer.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Remark = i.Remark
        }).ToList();

        if (items.Any())
        {
            await _unitOfWork.StockTransferItems.CreateRangeAsync(items);
            await _unitOfWork.CompleteAsync();
        }

        // Inventory movements: Out from source, In to destination
        if (items.Any())
        {
            foreach (var it in items)
            {
                // From warehouse: OUT
                await AdjustInventoryAsync(
                    warehouseId: transfer.FromWarehouseId,
                    productId: it.ProductId,
                    quantityDelta: -it.Quantity,
                    movementType: "StockTransfer",
                    direction: "Out",
                    movementDate: transfer.TransferDate,
                    referenceId: transfer.Id,
                    note: transfer.TransferNote
                );

                // To warehouse: IN
                await AdjustInventoryAsync(
                    warehouseId: transfer.ToWarehouseId,
                    productId: it.ProductId,
                    quantityDelta: it.Quantity,
                    movementType: "StockTransfer",
                    direction: "In",
                    movementDate: transfer.TransferDate,
                    referenceId: transfer.Id,
                    note: transfer.TransferNote
                );
            }

            await _unitOfWork.CompleteAsync();
        }

        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Done] };
    }

    public async Task<Response<StockTransferUpdateRequest>> UpdateAsync(int id, StockTransferUpdateRequest model)
    {
        if (id != model.Id) return new() { Message = "_sharLocalizer[Localization.InvalidId]" };

        var existing = await GetObjByIdAsync(id);
        if (existing is null) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        // revalidate warehouses
        bool fromExist = await _unitOfWork.Warehouses.ExistAsync(x => x.Id == model.FromWarehouseId && x.IsActive);
        if (!fromExist) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        bool toExist = await _unitOfWork.Warehouses.ExistAsync(x => x.Id == model.ToWarehouseId && x.IsActive);
        if (!toExist) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        if (!model.Items.Any()) return new() { Message = _sharLocalizer[Localization.Error] };

        var productIds = model.Items.Select(i => i.ProductId).Distinct().ToList();
        var existingProductsCount = await _unitOfWork.Product.CountAsync(x => productIds.Contains(x.Id));
        if (existingProductsCount != productIds.Count) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        // MVP: do not rollback/reapply inventory on update (TODO)
        await _unitOfWork.StockTransferItems.ExecuteDeleteAsync(x => x.StockTransferId == existing.Id);

        existing.FromWarehouseId = model.FromWarehouseId;
        existing.ToWarehouseId = model.ToWarehouseId;
        existing.TransferDate = model.TransferDate;
        existing.TransferCode = model.TransferCode ?? existing.TransferCode;
        existing.Remark = model.Remark;
        existing.TransferNote = model.TransferNote;

        var items = model.Items.Select(i => new StockTransferItem
        {
            StockTransferId = existing.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Remark = i.Remark
        }).ToList();

        existing.TotalQuantity = items.Sum(x => x.Quantity);

        _unitOfWork.StockTransfers.Update(existing);

        if (items.Any())
            await _unitOfWork.StockTransferItems.CreateRangeAsync(items);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Updated] };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await _unitOfWork.StockTransfers.GetByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        obj.IsActive = !obj.IsActive;
        _unitOfWork.StockTransfers.Update(obj);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = obj.IsActive ? _sharLocalizer[Localization.Activated] : _sharLocalizer[Localization.DeActivated] };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null) return new() { Message = _sharLocalizer[Localization.CannotBeFound] };

        // TODO: implement inventory rollback if required in future.

        _unitOfWork.StockTransfers.Delete(obj);

        // Remove items
        await _unitOfWork.StockTransferItems.ExecuteDeleteAsync(x => x.StockTransferId == id);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = _sharLocalizer[Localization.Deleted] };
    }
}