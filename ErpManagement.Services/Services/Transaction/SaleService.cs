using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Inventory;
using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class SaleService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                       IHubContext<BroadcastHub, IHubClient> hubContext) : ISaleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    #region Private Methods

    private async Task<Sale?> GetObjByIdAsync(int id)
    {
        return await _unitOfWork.Sales
            .GetFirstOrDefaultAsync(x => x.Id == id,
                includeProperties: "Customer,Warehouse,Branch,Biller,Items,Items.Product,Payments");
    }

    /// <summary>
    /// Adjust inventory for sale-related change.
    /// quantityDelta:
    ///   > 0  => stock IN
    ///   < 0  => stock OUT
    /// For Sale creation we will apply negative deltas (stock out).
    /// </summary>
    private async Task AdjustInventoryForSaleAsync(
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
                IsActive = true
            };

            await _unitOfWork.WarehouseProducts.CreateAsync(wp);
        }

        // apply delta
        wp.Quantity += quantityDelta;

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
            ReferenceType = "Sale",
            ReferenceId = referenceId,
            Note = note,
            IsActive = true
        };

        await _unitOfWork.StockMovements.CreateAsync(movement);
    }

    #endregion

    public async Task<Response<SaleGetAllResponse>> GetAllAsync(RequestLangEnum lang, SaleGetAllFiltrationsForSalesRequest model)
    {
        var total = await _unitOfWork.Sales.CountAsync();

        var items = (await _unitOfWork.Sales.GetSpecificSelectAsync(
            null,
            select: x => new PaginatedSalesData
            {
                Id = x.Id,
                SaleCode = x.SaleCode,
                CustomerName = x.Customer.FirstName,
                WarehouseName = x.Warehouse != null ? x.Warehouse.Name : null,
                BillerName = x.Biller != null ? x.Biller.FirstName : null,
                SaleDate = x.SaleDate,
                TotalAmount = x.TotalAmount,
                SaleStatus = x.SaleStatus,
                PaymentStatus = x.PaymentStatus,
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
                Data = new SaleGetAllResponse
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
            Data = new SaleGetAllResponse
            {
                Items = items,
                TotalRecords = total
            },
            IsSuccess = true
        };
    }

    public async Task<Response<SaleGetByIdResponse>> GetByIdAsync(int id)
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

        var dto = _mapper.Map<SaleGetByIdResponse>(obj);
        return new()
        {
            Data = dto,
            IsSuccess = true
        };
    }

    public async Task<Response<SaleGetByIdResponse>> CreateAsync(SaleCreateRequest model)
    {
        #region Validations

        // Customer must exist and be active
        bool customerExist = await _unitOfWork.Customer
            .ExistAsync(x => x.Id == model.CustomerId && x.IsActive);
        if (!customerExist)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                "_sharLocalizer[Localization.People.Customer]");
            return new() { Error = msg, Message = msg };
        }

        // Branch must exist and be active
        bool branchExist = await _unitOfWork.Branches
            .ExistAsync(x => x.Id == model.BranchId && x.IsActive);
        if (!branchExist)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                "_sharLocalizer[Localization.Organization.Branch]");
            return new() { Error = msg, Message = msg };
        }

        // Warehouse is required for POS MVP
        if (!model.WarehouseId.HasValue)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Error = msg, Message = msg };
        }

        // Warehouse must exist and be active
        var warehouse = await _unitOfWork.Warehouses
            .GetFirstOrDefaultAsync(x => x.Id == model.WarehouseId.Value && x.IsActive);
        if (warehouse is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                "_sharLocalizer[Localization.Organization.Warehouse]");
            return new() { Error = msg, Message = msg };
        }

        // Warehouse must belong to the specified branch
        if (warehouse.BranchId != model.BranchId)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Error = msg, Message = msg };
        }

        if (model.BillerId.HasValue)
        {
            bool billerExist = await _unitOfWork.Biller
                .ExistAsync(x => x.Id == model.BillerId.Value && x.IsActive);
            if (!billerExist)
            {
                string msg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                    "_sharLocalizer[Localization.People.Biller]");
                return new() { Error = msg, Message = msg };
            }
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
        }

        // Inventory validation — check stock before creating sale
        // Also capture AverageCost snapshot for COGS calculation
        var productCosts = new Dictionary<int, decimal>();
        foreach (var it in model.Items)
        {
            var wp = await _unitOfWork.WarehouseProducts
                .GetFirstOrDefaultAsync(x =>
                    x.WarehouseId == model.WarehouseId.Value &&
                    x.ProductId == it.ProductId);

            decimal available = wp?.Quantity ?? 0;
            if (available < it.Quantity)
            {
                string msg = _sharLocalizer[Localization.Error];
                return new() { Error = msg, Message = msg };
            }

            productCosts[it.ProductId] = wp?.AverageCost ?? 0;
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

        #region Determine PaymentStatus

        string paymentStatus;
        if (model.PaidAmount <= 0)
            paymentStatus = "Unpaid";
        else if (model.PaidAmount < total)
            paymentStatus = "PartiallyPaid";
        else
            paymentStatus = "Paid";

        #endregion

        #region Create Sale

        var sale = new Sale
        {
            CustomerId = model.CustomerId,
            WarehouseId = model.WarehouseId,
            BranchId = model.BranchId,
            BillerId = model.BillerId,
            SaleCode = string.Empty,
            SaleDate = model.SaleDate,
            ShippingAmount = model.ShippingAmount,
            Amount = amount,
            TaxAmount = taxAmount,
            Discount = discount,
            TotalAmount = total,
            Note = model.Note,
            SaleStatus = "Pending",
            PaymentStatus = paymentStatus,
            IsActive = true
        };

        await _unitOfWork.Sales.CreateAsync(sale);
        await _unitOfWork.CompleteAsync(); // ensure Id

        // Generate SaleCode using the real Id
        sale.SaleCode = $"INV-{sale.BranchId}-{sale.Id:000000}";
        _unitOfWork.Sales.Update(sale);
        await _unitOfWork.CompleteAsync();

        #endregion

        #region Create Sale Items

        var items = model.Items.Select(i => new SaleItem
        {
            SaleId = sale.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Amount = i.Quantity * i.UnitPrice,
            TaxAmount = i.TaxAmount ?? 0,
            Discount = i.Discount ?? 0,
            TotalAmount = (i.Quantity * i.UnitPrice) + (i.TaxAmount ?? 0) - (i.Discount ?? 0),
            CostAtSale = productCosts.GetValueOrDefault(i.ProductId, 0)
        }).ToList();

        if (items.Any())
        {
            await _unitOfWork.SaleItems.CreateRangeAsync(items);
            await _unitOfWork.CompleteAsync();
        }

        #endregion

        #region Inventory (STOCK OUT)

        if (items.Any())
        {
            var warehouseId = sale.WarehouseId!.Value;

            foreach (var it in items)
            {
                await AdjustInventoryForSaleAsync(
                    warehouseId: warehouseId,
                    productId: it.ProductId,
                    quantityDelta: -it.Quantity,
                    movementType: "Sale",
                    direction: "Out",
                    movementDate: sale.SaleDate,
                    referenceId: sale.Id,
                    note: sale.Note
                );
            }

            await _unitOfWork.CompleteAsync();
        }

        #endregion

        #region Payment

        if (model.PaidAmount > 0)
        {
            decimal actualPaid = Math.Min(model.PaidAmount, total);

            var payment = new Payment
            {
                SaleId = sale.Id,
                PayableAmount = total,
                PaidAmount = actualPaid,
                PaymentType = model.PaymentType,
                PaymentDate = sale.SaleDate,
                TransactionNumber = model.TransactionNumber,
                AccountNumber = model.AccountNumber,
                PaymentCode = string.Empty,
                IsActive = true
            };

            await _unitOfWork.Payments.CreateAsync(payment);
            await _unitOfWork.CompleteAsync();

            payment.PaymentCode = $"PAY-{sale.BranchId}-{payment.Id:000000}";
            _unitOfWork.Payments.Update(payment);
            await _unitOfWork.CompleteAsync();

            // Auto-create CashMovement for Cash payments
            if (string.Equals(model.PaymentType, "Cash", StringComparison.OrdinalIgnoreCase))
            {
                var cashbox = await _unitOfWork.Cashboxes
                    .GetFirstOrDefaultAsync(x => x.BranchId == sale.BranchId && x.IsActive);

                if (cashbox is null)
                {
                    return new()
                    {
                        Error = "No cashbox found for this branch.",
                        Message = "No cashbox found for this branch."
                    };
                }

                var openShift = await _unitOfWork.CashboxShifts
                    .GetFirstOrDefaultAsync(x => x.CashboxId == cashbox.Id && !x.IsClosed);

                if (openShift is null)
                {
                    return new()
                    {
                        Error = "No open cashbox shift for this branch.",
                        Message = "No open cashbox shift for this branch."
                    };
                }

                var cashMovement = new CashMovement
                {
                    CashboxShiftId = openShift.Id,
                    Type = CashMovementType.CashIn,
                    Amount = actualPaid,
                    Reason = "Sale payment",
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = sale.UserId ?? string.Empty,
                    ReferenceType = "SalePayment",
                    ReferenceId = payment.Id,
                    IsActive = true
                };

                await _unitOfWork.CashMovements.CreateAsync(cashMovement);
                await _unitOfWork.CompleteAsync();
            }
        }

        #endregion

        await _hubContext.Clients.All.BroadcastMessage();

        var obj = await GetObjByIdAsync(sale.Id);
        var dto = _mapper.Map<SaleGetByIdResponse>(obj!);
        return new()
        {
            Data = dto,
            IsSuccess = true,
            Message = _sharLocalizer[Localization.Done]
        };
    }

    public async Task<Response<SaleUpdateRequest>> UpdateAsync(int id, SaleUpdateRequest model)
    {
        var existing = await GetObjByIdAsync(id);
        if (existing is null || id != model.Id)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        // Remove old items (MVP) and rebuild items/headers. Inventory rollback not implemented for MVP.
        // TODO: implement inventory rollback + reapply on update (future enhancement).

        await _unitOfWork.SaleItems.ExecuteDeleteAsync(x => x.SaleId == existing.Id);

        existing.CustomerId = model.CustomerId;
        existing.WarehouseId = model.WarehouseId;
        existing.BranchId = model.BranchId;
        existing.BillerId = model.BillerId;
        existing.SaleDate = model.SaleDate;
        existing.SaleCode = model.SaleCode ?? existing.SaleCode;
        existing.ShippingAmount = model.ShippingAmount;
        existing.Note = model.Note;

        decimal amount = 0M;
        decimal taxAmount = 0M;
        decimal discount = 0M;

        var items = model.Items.Select(i => new SaleItem
        {
            SaleId = existing.Id,
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

        _unitOfWork.Sales.Update(existing);
        if (items.Any())
            await _unitOfWork.SaleItems.CreateRangeAsync(items);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Updated] };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await _unitOfWork.Sales.GetByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Sales.Update(obj);
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

        // TODO: implement stock reversal for sale delete if required.

        _unitOfWork.Sales.Delete(obj);

        await _unitOfWork.SaleItems.ExecuteDeleteAsync(x => x.SaleId == id);

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = _sharLocalizer[Localization.Deleted] };
    }
}