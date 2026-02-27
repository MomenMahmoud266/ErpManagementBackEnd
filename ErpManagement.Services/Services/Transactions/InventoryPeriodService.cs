using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Models.Inventory;

namespace ErpManagement.Services.Services.Transactions;

public class InventoryPeriodService(IUnitOfWork unitOfWork, ICurrentTenant currentTenant) : IInventoryPeriodService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentTenant _currentTenant = currentTenant;

    private static InventoryPeriodGetByIdResponse MapToResponse(InventoryPeriod p) => new()
    {
        Id = p.Id,
        BranchId = p.BranchId,
        BranchName = p.Branch?.NameEn ?? string.Empty,
        From = p.From,
        To = p.To,
        IsClosed = p.IsClosed,
        ClosedAt = p.ClosedAt,
        BeginningValue = p.BeginningValue,
        PurchasesValue = p.PurchasesValue,
        EndingValue = p.EndingValue,
        CogsValue = p.CogsValue,
        PhysicalCounts = p.PhysicalCounts?.Select(c => new PhysicalCountDto
        {
            Id = c.Id,
            WarehouseId = c.WarehouseId,
            ProductId = c.ProductId,
            CountQty = c.CountQty,
            CostUsed = c.CostUsed,
            LineValue = c.LineValue
        }).ToList() ?? new()
    };

    public async Task<Response<InventoryPeriodGetAllResponse>> GetAllAsync(int? branchId)
    {
        var items = (await _unitOfWork.InventoryPeriods.GetAllAsync(
            x => x.TenantId == _currentTenant.TenantId &&
                 (!branchId.HasValue || x.BranchId == branchId),
            includeProperties: "Branch,PhysicalCounts"))
            .ToList();

        return new()
        {
            IsSuccess = true,
            Data = new InventoryPeriodGetAllResponse
            {
                Items = items.Select(MapToResponse).ToList(),
                TotalRecords = items.Count
            }
        };
    }

    public async Task<Response<InventoryPeriodGetByIdResponse>> GetByIdAsync(int id)
    {
        var period = await _unitOfWork.InventoryPeriods
            .GetFirstOrDefaultAsync(x => x.Id == id && x.TenantId == _currentTenant.TenantId,
                includeProperties: "Branch,PhysicalCounts");

        if (period is null)
            return new() { Error = "Period not found.", Message = "Period not found." };

        return new() { IsSuccess = true, Data = MapToResponse(period) };
    }

    public async Task<Response<InventoryPeriodGetByIdResponse>> CreateAsync(InventoryPeriodCreateRequest model)
    {
        bool branchExists = await _unitOfWork.Branches.ExistAsync(x => x.Id == model.BranchId && x.IsActive);
        if (!branchExists)
            return new() { Error = "Branch not found.", Message = "Branch not found." };

        var period = new InventoryPeriod
        {
            TenantId = _currentTenant.TenantId,
            BranchId = model.BranchId,
            From = model.From,
            To = model.To,
            IsActive = true
        };

        await _unitOfWork.InventoryPeriods.CreateAsync(period);
        await _unitOfWork.CompleteAsync();

        var created = await _unitOfWork.InventoryPeriods
            .GetFirstOrDefaultAsync(x => x.Id == period.Id, includeProperties: "Branch,PhysicalCounts");

        return new() { IsSuccess = true, Data = MapToResponse(created!) };
    }

    public async Task<Response<InventoryPeriodGetByIdResponse>> AddPhysicalCountAsync(
        int periodId, PhysicalCountCreateRequest model)
    {
        var period = await _unitOfWork.InventoryPeriods
            .GetFirstOrDefaultAsync(x => x.Id == periodId && x.TenantId == _currentTenant.TenantId,
                includeProperties: "Branch,PhysicalCounts");

        if (period is null)
            return new() { Error = "Period not found.", Message = "Period not found." };

        if (period.IsClosed)
            return new() { Error = "Cannot add counts to a closed period.", Message = "Cannot add counts to a closed period." };

        // Remove existing count for same warehouse+product if present
        var existing = period.PhysicalCounts
            .FirstOrDefault(c => c.WarehouseId == model.WarehouseId && c.ProductId == model.ProductId);

        if (existing is not null)
        {
            await _unitOfWork.PhysicalCounts.ExecuteDeleteAsync(
                x => x.Id == existing.Id);
        }

        decimal lineValue = model.CountQty * model.CostUsed;
        var count = new PhysicalCount
        {
            TenantId = _currentTenant.TenantId,
            InventoryPeriodId = periodId,
            WarehouseId = model.WarehouseId,
            ProductId = model.ProductId,
            CountQty = model.CountQty,
            CostUsed = model.CostUsed,
            LineValue = lineValue,
            IsActive = true
        };

        await _unitOfWork.PhysicalCounts.CreateAsync(count);
        await _unitOfWork.CompleteAsync();

        // Reload
        period = await _unitOfWork.InventoryPeriods
            .GetFirstOrDefaultAsync(x => x.Id == periodId, includeProperties: "Branch,PhysicalCounts");

        return new() { IsSuccess = true, Data = MapToResponse(period!) };
    }

    public async Task<Response<InventoryPeriodGetByIdResponse>> CloseAsync(int periodId)
    {
        var period = await _unitOfWork.InventoryPeriods
            .GetFirstOrDefaultAsync(x => x.Id == periodId && x.TenantId == _currentTenant.TenantId,
                includeProperties: "Branch,PhysicalCounts");

        if (period is null)
            return new() { Error = "Period not found.", Message = "Period not found." };

        if (period.IsClosed)
            return new() { Error = "Period is already closed.", Message = "Period is already closed." };

        // Step 1: BeginningValue = EndingValue of previous closed period for this branch, or 0
        var previousPeriod = (await _unitOfWork.InventoryPeriods.GetAllAsync(
            x => x.TenantId == _currentTenant.TenantId &&
                 x.BranchId == period.BranchId &&
                 x.IsClosed &&
                 x.To < period.From))
            .OrderByDescending(p => p.To)
            .FirstOrDefault();

        period.BeginningValue = previousPeriod?.EndingValue ?? 0;

        // Step 2: PurchasesValue = sum of purchase item costs for this branch's warehouses in period
        var branchWarehouses = (await _unitOfWork.Warehouses.GetAllAsync(
            x => x.BranchId == period.BranchId && x.IsActive))
            .Select(w => w.Id)
            .ToList();

        decimal purchasesValue = 0;
        if (branchWarehouses.Any())
        {
            var purchasesInPeriod = await _unitOfWork.Purchases.GetAllAsync(
                x => x.TenantId == _currentTenant.TenantId &&
                     x.WarehouseId.HasValue &&
                     branchWarehouses.Contains(x.WarehouseId!.Value) &&
                     x.PurchaseDate >= period.From &&
                     x.PurchaseDate <= period.To,
                includeProperties: "Items");

            purchasesValue = purchasesInPeriod
                .SelectMany(p => p.Items)
                .Sum(i => i.Quantity * i.UnitPrice);
        }
        period.PurchasesValue = purchasesValue;

        // Step 3: EndingValue = sum(CountQty * CostUsed) from PhysicalCounts
        period.EndingValue = period.PhysicalCounts.Sum(c => c.LineValue);

        // Step 4: COGS = Beginning + Purchases - Ending
        period.CogsValue = period.BeginningValue + period.PurchasesValue - period.EndingValue;

        // Step 5: Mark period closed
        period.IsClosed = true;
        period.ClosedAt = DateTime.UtcNow;

        _unitOfWork.InventoryPeriods.Update(period);
        await _unitOfWork.CompleteAsync();

        return new() { IsSuccess = true, Data = MapToResponse(period) };
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var period = await _unitOfWork.InventoryPeriods
            .GetFirstOrDefaultAsync(x => x.Id == id && x.TenantId == _currentTenant.TenantId);

        if (period is null)
            return new() { Error = "Period not found.", Message = "Period not found." };

        if (period.IsClosed)
            return new() { Error = "Cannot delete a closed period.", Message = "Cannot delete a closed period." };

        await _unitOfWork.PhysicalCounts.ExecuteDeleteAsync(x => x.InventoryPeriodId == id);
        _unitOfWork.InventoryPeriods.Delete(period);
        await _unitOfWork.CompleteAsync();

        return new() { IsSuccess = true, Message = "Deleted" };
    }
}
