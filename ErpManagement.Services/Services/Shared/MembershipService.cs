using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request;
using ErpManagement.Domain.DTOs.Request.Shared.Gym;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Shared.Gym;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Models.Gym;
using ErpManagement.Domain.Models.Products;
using ErpManagement.Services.IServices.Shared;
using ErpManagement.Services.IServices.Transactions;
using ErpManagement.Services.IServices.WebSocket;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using static ErpManagement.Domain.Constants.Statics.SDStatic;

namespace ErpManagement.Services.Services.Shared;

public class MembershipService(
    IUnitOfWork unitOfWork,
    IStringLocalizer<SharedResource> sharLocalizer,
    IHubContext<BroadcastHub, IHubClient> hubContext,
    ISaleService saleService,
    ICurrentTenant currentTenant) : IMembershipService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;
    private readonly ISaleService _saleService = saleService;
    private readonly ICurrentTenant _currentTenant = currentTenant;

    public async Task<Response<MembershipPlanGetAllResponse>> GetAllPlansAsync(RequestLangEnum lang, PaginationRequest model, int? branchId)
    {
        var totalRecords = await _unitOfWork.MembershipPlans.CountAsync(
            x => !branchId.HasValue || x.BranchId == branchId);

        var items = (await _unitOfWork.MembershipPlans.GetSpecificSelectAsync(
            filter: x => !branchId.HasValue || x.BranchId == branchId,
            select: x => new PaginatedMembershipPlansData
            {
                Id = x.Id,
                BranchId = x.BranchId,
                Name = x.Name,
                DurationDays = x.DurationDays,
                Price = x.Price,
                ProductId = x.ProductId,
                IsActive = x.IsActive
            },
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            orderBy: q => q.OrderByDescending(x => x.Id)
        )).ToList();

        var result = new MembershipPlanGetAllResponse
        {
            TotalRecords = totalRecords,
            Items = items
        };

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<MembershipPlanCreateRequest>> CreatePlanAsync(MembershipPlanCreateRequest model)
    {
        bool branchExists = await _unitOfWork.Branches.ExistAsync(x => x.Id == model.BranchId && x.IsActive);
        if (!branchExists)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Branch]);
            return new() { Message = msg, Error = msg };
        }

        bool categoryExists = await _unitOfWork.Categories.ExistAsync(x => x.Id == model.CategoryId);
        if (!categoryExists)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Category]);
            return new() { Message = msg, Error = msg };
        }

        // Create a product representing the membership plan
        var product = new Product
        {
            CategoryId = model.CategoryId,
            ProductCode = $"MEM-{model.BranchId}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
            Title = model.Name,
            Price = model.Price,
            TypeId = model.ProductTypeId,
            TaxId = model.TaxId,
            Quantity = 0,
            IsActive = true
        };

        await _unitOfWork.Product.CreateAsync(product);
        await _unitOfWork.CompleteAsync();

        var plan = new MembershipPlan
        {
            BranchId = model.BranchId,
            Name = model.Name,
            DurationDays = model.DurationDays,
            Price = model.Price,
            ProductId = product.Id,
            IsActive = true
        };

        await _unitOfWork.MembershipPlans.CreateAsync(plan);
        await _unitOfWork.CompleteAsync();

        await _hubContext.Clients.All.BroadcastMessage();
        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Done] };
    }

    public async Task<Response<MembershipPlanUpdateRequest>> UpdatePlanAsync(int id, MembershipPlanUpdateRequest model)
    {
        if (id != model.Id)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Message = msg, Error = msg };
        }

        var plan = await _unitOfWork.MembershipPlans.GetByIdAsync(id);
        if (plan is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Item]);
            return new() { Message = msg, Error = msg };
        }

        plan.BranchId = model.BranchId;
        plan.Name = model.Name;
        plan.DurationDays = model.DurationDays;
        plan.Price = model.Price;
        plan.IsActive = model.IsActive;

        _unitOfWork.MembershipPlans.Update(plan);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Updated] };
    }

    public async Task<Response<object>> DeletePlanAsync(int id)
    {
        var plan = await _unitOfWork.MembershipPlans.GetByIdAsync(id);
        if (plan is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Item]);
            return new() { Message = msg, Error = msg };
        }

        _unitOfWork.MembershipPlans.Delete(plan);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = _sharLocalizer[Localization.Deleted] };
    }

    public async Task<Response<SaleGetByIdResponse>> PurchaseMembershipAsync(PurchaseMembershipRequest model)
    {
        var plan = await _unitOfWork.MembershipPlans.GetByIdAsync(model.MembershipPlanId);
        if (plan is null || !plan.IsActive)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Item]);
            return new() { Message = msg, Error = msg };
        }

        if (plan.BranchId != model.BranchId)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Branch]);
            return new() { Message = msg, Error = msg };
        }

        bool branchExists = await _unitOfWork.Branches.ExistAsync(x => x.Id == model.BranchId && x.IsActive);
        if (!branchExists)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Branch]);
            return new() { Message = msg, Error = msg };
        }

        bool warehouseExists = await _unitOfWork.Warehouses.ExistAsync(x => x.Id == model.WarehouseId && x.IsActive && x.BranchId == model.BranchId);
        if (!warehouseExists)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Warehouse]);
            return new() { Message = msg, Error = msg };
        }

        bool customerExists = await _unitOfWork.Customer.ExistAsync(x => x.Id == model.CustomerId && x.IsActive);
        if (!customerExists)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Customer]);
            return new() { Message = msg, Error = msg };
        }

        var now = DateTime.UtcNow;

        // Find existing active subscription for this customer / plan / branch
        var existingSubscription = await _unitOfWork.MemberSubscriptions.GetFirstOrDefaultAsync(
            x => x.CustomerId == model.CustomerId &&
                 x.MembershipPlanId == model.MembershipPlanId &&
                 x.BranchId == model.BranchId &&
                 x.Status == SubscriptionStatus.Active);

        // Build sale request
        var saleRequest = new SaleCreateRequest
        {
            BranchId = model.BranchId,
            WarehouseId = model.WarehouseId,
            CustomerId = model.CustomerId,
            BillerId = model.BillerId,
            PaidAmount = model.PaidAmount,
            PaymentType = model.PaymentType,
            TransactionNumber = model.TransactionNumber,
            AccountNumber = model.AccountNumber,
            SaleDate = now,
            Items = new List<SaleItemCreateRequest>
            {
                new()
                {
                    ProductId = plan.ProductId,
                    Quantity = 1,
                    UnitPrice = plan.Price,
                    Discount = 0
                }
            }
        };

        var saleResult = await _saleService.CreateAsync(saleRequest);
        if (!saleResult.IsSuccess)
            return new() { Message = saleResult.Message, Error = saleResult.Error };

        if (existingSubscription is not null && existingSubscription.EndAt >= now)
        {
            // Renew: extend end date
            existingSubscription.EndAt = existingSubscription.EndAt.AddDays(plan.DurationDays);
            existingSubscription.LastSaleId = saleResult.Data.Id;
            _unitOfWork.MemberSubscriptions.Update(existingSubscription);
        }
        else
        {
            // New subscription
            var newSubscription = new MemberSubscription
            {
                BranchId = model.BranchId,
                CustomerId = model.CustomerId,
                MembershipPlanId = model.MembershipPlanId,
                StartAt = now,
                EndAt = now.AddDays(plan.DurationDays),
                Status = SubscriptionStatus.Active,
                LastSaleId = saleResult.Data.Id
            };
            await _unitOfWork.MemberSubscriptions.CreateAsync(newSubscription);
        }

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return saleResult;
    }

    public async Task<Response<CheckInResponse>> CheckInAsync(CheckInRequest model)
    {
        var now = DateTime.UtcNow;

        var subscription = await _unitOfWork.MemberSubscriptions.GetFirstOrDefaultAsync(
            x => x.CustomerId == model.CustomerId &&
                 x.BranchId == model.BranchId &&
                 x.Status == SubscriptionStatus.Active &&
                 x.StartAt <= now &&
                 x.EndAt >= now &&
                 (!model.MembershipPlanId.HasValue || x.MembershipPlanId == model.MembershipPlanId));

        if (subscription is null)
        {
            string msg = "No active membership";
            return new() { Message = msg, Error = msg };
        }

        var checkIn = new MemberCheckIn
        {
            BranchId = model.BranchId,
            CustomerId = model.CustomerId,
            MemberSubscriptionId = subscription.Id,
            CheckInAt = now
        };

        await _unitOfWork.MemberCheckIns.CreateAsync(checkIn);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new()
        {
            Data = new CheckInResponse
            {
                MemberSubscriptionId = subscription.Id,
                CheckInId = checkIn.Id,
                CheckInAt = checkIn.CheckInAt,
                SubscriptionStatus = subscription.Status,
                SubscriptionEndAt = subscription.EndAt
            },
            IsSuccess = true,
            Message = _sharLocalizer[Localization.Done]
        };
    }
}
