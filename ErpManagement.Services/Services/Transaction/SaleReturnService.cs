using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class SaleReturnService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                             IHubContext<BroadcastHub, IHubClient> hubContext) : ISaleReturnService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    private async Task<SaleReturn?> GetObjByIdAsync(int id)
    {
        return await _unitOfWork.SaleReturns
            .GetFirstOrDefaultAsync(x => x.Id == id,
                includeProperties: "Customer,Warehouse,Biller,Sale,Items,Items.Product");
    }

    public async Task<Response<SaleReturnGetAllResponse>> GetAllAsync(RequestLangEnum lang, SaleReturnGetAllFiltrationsForSaleReturnsRequest model)
    {
        var total = await _unitOfWork.SaleReturns.CountAsync();

        var items = (await _unitOfWork.SaleReturns.GetSpecificSelectAsync(
            null,
            select: x => new PaginatedSaleReturnsData
            {
                Id = x.Id,
                ReturnCode = x.ReturnCode,
                SaleCode = x.Sale != null ? x.Sale.SaleCode : null,
                CustomerName = x.Customer.FirstName,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : null,
                BillerName = x.Biller != null ? x.Biller.FirstName : null,
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
                Data = new SaleReturnGetAllResponse
                {
                    Items = Array.Empty<PaginatedSaleReturnsData>(),
                    TotalRecords = 0
                },
                Message = msg,
                Error = msg
            };
        }

        return new()
        {
            Data = new SaleReturnGetAllResponse
            {
                Items = items,
                TotalRecords = total
            },
            IsSuccess = true
        };
    }

    public async Task<Response<SaleReturnGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Data = null!, Message = msg, Error = msg };
        }

        var dto = _mapper.Map<SaleReturnGetByIdResponse>(obj);
        return new() { Data = dto, IsSuccess = true };
    }

    public async Task<Response<SaleReturnCreateRequest>> CreateAsync(SaleReturnCreateRequest model)
    {
        #region Validations

        // Sale must exist (with items)
        var sale = await _unitOfWork.Sales.GetFirstOrDefaultAsync(
            x => x.Id == model.SaleId,
            includeProperties: "Items"
        );
        if (sale is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.Shared.Sale]" ?? "Sale");
            return new() { Error = msg, Message = msg };
        }

        // Customer must exist and match sale customer
        bool customerExist = await _unitOfWork.Customer.ExistAsync(x => x.Id == model.CustomerId && x.IsActive);
        if (!customerExist || sale.CustomerId != model.CustomerId)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.People.Customer]");
            return new() { Error = msg, Message = msg };
        }

        if (model.WarehouseId.HasValue)
        {
            bool whExist = await _unitOfWork.Warehouses.ExistAsync(x => x.Id == model.WarehouseId.Value && x.IsActive);
            if (!whExist)
            {
                string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.Organization.Warehouse]");
                return new() { Error = msg, Message = msg };
            }
        }

        if (model.BillerId.HasValue)
        {
            bool billerExist = await _unitOfWork.Biller.ExistAsync(x => x.Id == model.BillerId.Value && x.IsActive);
            if (!billerExist)
            {
                string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.People.Biller]");
                return new() { Error = msg, Message = msg };
            }
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
            if (it.Quantity <= 0 || it.UnitPrice < 0)
            {
                string msg = _sharLocalizer[Localization.Error];
                return new() { Error = msg, Message = msg };
            }

            var saleLine = sale.Items?.FirstOrDefault(si => si.ProductId == it.ProductId);
            if (saleLine is null || it.Quantity > saleLine.Quantity)
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

        #region Create SaleReturn
        var sr = new SaleReturn
        {
            SaleId = model.SaleId,
            CustomerId = model.CustomerId,
            WarehouseId = model.WarehouseId ?? sale.WarehouseId,
            BillerId = model.BillerId ?? sale.BillerId,
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

        await _unitOfWork.SaleReturns.CreateAsync(sr);
        await _unitOfWork.CompleteAsync(); // ensure Id
        #endregion

        #region Create Items
        var items = model.Items.Select(i => new SaleReturnItem
        {
            SaleReturnId = sr.Id,
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
            await _unitOfWork.SaleReturnItems.CreateRangeAsync(items);
            await _unitOfWork.CompleteAsync();
        }
        #endregion

        #region Inventory effect: STOCK IN (returns increase stock)
        if (items.Any() && sr.WarehouseId.HasValue)
        {
            var warehouseId = sr.WarehouseId.Value;
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

                wp.Quantity += it.Quantity;        // stock IN for sale return
                if (wp.Quantity < 0) wp.Quantity = 0;
                _unitOfWork.WarehouseProducts.Update(wp);

                var movement = new StockMovement
                {
                    ProductId = it.ProductId,
                    WarehouseId = warehouseId,
                    MovementType = "SaleReturn",
                    Direction = "In",
                    Quantity = it.Quantity,
                    BalanceAfter = wp.Quantity,
                    MovementDate = sr.ReturnDate,
                    ReferenceType = "SaleReturn",
                    ReferenceId = sr.Id,
                    Note = sr.ReturnNote,
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

        #region Cash-Out Movement for Cash Refunds
        if (string.Equals(model.RefundType, "Cash", StringComparison.OrdinalIgnoreCase)
            && model.RefundAmount > 0)
        {
            var cashbox = await _unitOfWork.Cashboxes
                .GetFirstOrDefaultAsync(x => x.BranchId == sale.BranchId && x.IsActive);

            if (cashbox is not null)
            {
                var openShift = await _unitOfWork.CashboxShifts
                    .GetFirstOrDefaultAsync(x => x.CashboxId == cashbox.Id && !x.IsClosed);

                if (openShift is not null)
                {
                    var cashMovement = new CashMovement
                    {
                        CashboxShiftId = openShift.Id,
                        Type = CashMovementType.CashOut,
                        Amount = model.RefundAmount,
                        Reason = "Sale return refund",
                        CreatedAt = DateTime.UtcNow,
                        CreatedByUserId = sr.UserId ?? string.Empty,
                        ReferenceType = "SaleReturn",
                        ReferenceId = sr.Id,
                        IsActive = true
                    };

                    await _unitOfWork.CashMovements.CreateAsync(cashMovement);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }
        #endregion

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Done] };
    }

    public async Task<Response<SaleReturnUpdateRequest>> UpdateAsync(int id, SaleReturnUpdateRequest model)
    {
        var existing = await GetObjByIdAsync(id);
        if (existing is null || id != model.Id)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        // MVP: Do not adjust inventory for update (TODO)
        await _unitOfWork.SaleReturnItems.ExecuteDeleteAsync(x => x.SaleReturnId == existing.Id);

        existing.CustomerId = model.CustomerId;
        existing.WarehouseId = model.WarehouseId;
        existing.BillerId = model.BillerId;
        existing.SaleId = model.SaleId;
        existing.ReturnCode = model.ReturnCode ?? existing.ReturnCode;
        existing.ReturnDate = model.ReturnDate;
        existing.ShippingAmount = model.ShippingAmount;
        existing.Remark = model.Remark;
        existing.ReturnNote = model.ReturnNote;

        decimal amount = 0M;
        decimal taxAmount = 0M;
        decimal discount = 0M;

        var items = model.Items.Select(i => new SaleReturnItem
        {
            SaleReturnId = existing.Id,
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

        _unitOfWork.SaleReturns.Update(existing);
        if (items.Any()) await _unitOfWork.SaleReturnItems.CreateRangeAsync(items);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Updated] };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await _unitOfWork.SaleReturns.GetByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.SaleReturns.Update(obj);
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

        // MVP: Do NOT rollback inventory on delete (TODO)
        _unitOfWork.SaleReturns.Delete(obj);
        await _unitOfWork.SaleReturnItems.ExecuteDeleteAsync(x => x.SaleReturnId == id);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = _sharLocalizer[Localization.Deleted] };
    }
}