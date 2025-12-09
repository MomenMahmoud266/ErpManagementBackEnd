using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;

namespace ErpManagement.Services.Services.Transactions;

public class PaymentService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper,
                           IHubContext<BroadcastHub, IHubClient> hubContext) : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;

    private async Task<Payment?> GetObjByIdAsync(int id)
    {
        return await _unitOfWork.Payments
            .GetFirstOrDefaultAsync(x => x.Id == id,
                includeProperties: "Sale,Purchase");
    }

    public async Task<Response<PaymentGetAllResponse>> GetAllAsync(RequestLangEnum lang, PaymentGetAllFiltrationsForPaymentsRequest model)
    {
        var total = await _unitOfWork.Payments.CountAsync();

        // build predicate
        Expression<Func<Payment, bool>>? filter = null;
        if (model.SaleId.HasValue)
            filter = CombineFilter(filter, x => x.SaleId == model.SaleId.Value);
        if (model.PurchaseId.HasValue)
            filter = CombineFilter(filter, x => x.PurchaseId == model.PurchaseId.Value);
        if (model.FromDate.HasValue)
            filter = CombineFilter(filter, x => x.PaymentDate >= model.FromDate.Value.Date);
        if (model.ToDate.HasValue)
            filter = CombineFilter(filter, x => x.PaymentDate <= model.ToDate.Value.Date.AddDays(1).AddTicks(-1));
        if (!string.IsNullOrWhiteSpace(model.PaymentType))
            filter = CombineFilter(filter, x => x.PaymentType == model.PaymentType);

        var items = (await _unitOfWork.Payments.GetSpecificSelectAsync(
            filter,
            select: x => new PaginatedPaymentsData
            {
                Id = x.Id,
                PaymentCode = x.PaymentCode,
                PaymentDate = x.PaymentDate,
                PaidAmount = x.PaidAmount,
                PayableAmount = x.PayableAmount,
                PaymentType = x.PaymentType,
                SaleCode = x.Sale != null ? x.Sale.SaleCode : null,
                PurchaseCode = x.Purchase != null ? x.Purchase.PurchaseCode : null,
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
                Data = new PaymentGetAllResponse
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
            Data = new PaymentGetAllResponse
            {
                Items = items,
                TotalRecords = total
            },
            IsSuccess = true
        };
    }

    public async Task<Response<PaymentGetByIdResponse>> GetByIdAsync(int id)
    {
        var obj = await GetObjByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Data = null!, Message = msg, Error = msg };
        }

        var dto = _mapper.Map<PaymentGetByIdResponse>(obj);
        return new() { Data = dto, IsSuccess = true };
    }

    public async Task<Response<PaymentCreateRequest>> CreateAsync(PaymentCreateRequest model)
    {
        #region Validations
        if (!model.SaleId.HasValue && !model.PurchaseId.HasValue)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Error = msg, Message = msg };
        }

        if (model.SaleId.HasValue && model.PurchaseId.HasValue)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Error = msg, Message = msg };
        }

        if (model.PaidAmount <= 0 || model.PayableAmount < model.PaidAmount)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Error = msg, Message = msg };
        }

        Sale? sale = null;
        Purchase? purchase = null;

        if (model.SaleId.HasValue)
        {
            sale = await _unitOfWork.Sales.GetFirstOrDefaultAsync(x => x.Id == model.SaleId.Value, includeProperties: "Items");
            if (sale is null)
            {
                string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.Shared.Sale]" ?? "Sale");
                return new() { Error = msg, Message = msg };
            }

            // prevent overpayment
            var existingPayments = (await _unitOfWork.Payments.GetBySaleIdAsync(sale.Id)).ToList();
            var alreadyPaid = existingPayments.Sum(p => p.PaidAmount);
            if (alreadyPaid + model.PaidAmount > sale.TotalAmount)
            {
                string msg = _sharLocalizer[Localization.Error];
                return new() { Error = msg, Message = msg };
            }
        }

        if (model.PurchaseId.HasValue)
        {
            purchase = await _unitOfWork.Purchases.GetByIdAsync(model.PurchaseId.Value);
            if (purchase is null)
            {
                string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], "_sharLocalizer[Localization.Shared.Purchase]" ?? "Purchase");
                return new() { Error = msg, Message = msg };
            }
            // For MVP, no overpayment prevention for purchase (TODO)
        }
        #endregion

        var payment = new Payment
        {
            SaleId = model.SaleId,
            PurchaseId = model.PurchaseId,
            PaymentCode = model.PaymentCode ?? string.Empty,
            PaymentDate = model.PaymentDate,
            PaidAmount = model.PaidAmount,
            PayableAmount = model.PayableAmount,
            PaymentType = model.PaymentType,
            AccountNumber = model.AccountNumber,
            TransactionNumber = model.TransactionNumber,
            Remark = model.Remark,
            IsActive = true
        };

        await _unitOfWork.Payments.CreateAsync(payment);
        await _unitOfWork.CompleteAsync();

        // update sale payment status if needed
        if (sale is not null)
        {
            var payments = (await _unitOfWork.Payments.GetBySaleIdAsync(sale.Id)).ToList();
            var totalPaid = payments.Sum(p => p.PaidAmount);

            if (totalPaid <= 0) sale.PaymentStatus = "Unpaid";
            else if (totalPaid < sale.TotalAmount) sale.PaymentStatus = "Partial";
            else sale.PaymentStatus = "Paid";

            _unitOfWork.Sales.Update(sale);
            await _unitOfWork.CompleteAsync();
        }

        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Done] };
    }

    public async Task<Response<PaymentUpdateRequest>> UpdateAsync(int id, PaymentUpdateRequest model)
    {
        var existing = await GetObjByIdAsync(id);
        if (existing is null || id != model.Id)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        if (model.PaidAmount <= 0 || model.PayableAmount < model.PaidAmount)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Error = msg, Message = msg };
        }

        existing.PaymentDate = model.PaymentDate;
        existing.PaymentType = model.PaymentType;
        existing.PaidAmount = model.PaidAmount;
        existing.PayableAmount = model.PayableAmount;
        existing.AccountNumber = model.AccountNumber;
        existing.TransactionNumber = model.TransactionNumber;
        existing.Remark = model.Remark;
        existing.PaymentCode = model.PaymentCode ?? existing.PaymentCode;

        _unitOfWork.Payments.Update(existing);
        await _unitOfWork.CompleteAsync();

        // Recalculate sale payment status if payment relates to a sale
        if (existing.SaleId.HasValue)
        {
            var sale = await _unitOfWork.Sales.GetByIdAsync(existing.SaleId.Value);
            if (sale is not null)
            {
                var payments = (await _unitOfWork.Payments.GetBySaleIdAsync(sale.Id)).ToList();
                var totalPaid = payments.Sum(p => p.PaidAmount);

                if (totalPaid <= 0) sale.PaymentStatus = "Unpaid";
                else if (totalPaid < sale.TotalAmount) sale.PaymentStatus = "Partial";
                else sale.PaymentStatus = "Paid";

                _unitOfWork.Sales.Update(sale);
                await _unitOfWork.CompleteAsync();
            }
        }

        await _hubContext.Clients.All.BroadcastMessage();
        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Updated] };
    }

    public async Task<Response<string>> UpdateActiveOrNotAsync(int id)
    {
        var obj = await _unitOfWork.Payments.GetByIdAsync(id);
        if (obj is null)
        {
            string msg = _sharLocalizer[Localization.CannotBeFound];
            return new() { Error = msg, Message = msg };
        }

        obj.IsActive = !obj.IsActive;
        _unitOfWork.Payments.Update(obj);
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

        _unitOfWork.Payments.Delete(obj);
        await _unitOfWork.CompleteAsync();

        // update sale payment status if needed
        if (obj.SaleId.HasValue)
        {
            var sale = await _unitOfWork.Sales.GetByIdAsync(obj.SaleId.Value);
            if (sale is not null)
            {
                var payments = (await _unitOfWork.Payments.GetBySaleIdAsync(sale.Id)).ToList();
                var totalPaid = payments.Sum(p => p.PaidAmount);

                if (totalPaid <= 0) sale.PaymentStatus = "Unpaid";
                else if (totalPaid < sale.TotalAmount) sale.PaymentStatus = "Partial";
                else sale.PaymentStatus = "Paid";

                _unitOfWork.Sales.Update(sale);
                await _unitOfWork.CompleteAsync();
            }
        }

        await _hubContext.Clients.All.BroadcastMessage();
        return new() { IsSuccess = true, Message = _sharLocalizer[Localization.Deleted] };
    }

    // helper to combine expression filters
    private static Expression<Func<T, bool>>? CombineFilter<T>(Expression<Func<T, bool>>? existing, Expression<Func<T, bool>> add)
    {
        if (existing is null) return add;
        var invoked = Expression.Invoke(add, existing.Parameters);
        var body = Expression.AndAlso(existing.Body, invoked);
        return Expression.Lambda<Func<T, bool>>(body, existing.Parameters);
    }
}