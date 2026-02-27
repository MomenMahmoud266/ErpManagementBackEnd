using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Models.Transactions;
using ErpManagement.Services.IServices.Transactions;
using static ErpManagement.Domain.Constants.Statics.SDStatic;

namespace ErpManagement.Services.Services.Transactions;

public class CashboxService(IUnitOfWork unitOfWork) : ICashboxService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Response<CashboxGetAllResponse>> GetAllCashboxesAsync(
        RequestLangEnum lang, PaginationRequest model, int? branchId)
    {
        var total = await _unitOfWork.Cashboxes.CountAsync(
            x => !branchId.HasValue || x.BranchId == branchId);

        var items = (await _unitOfWork.Cashboxes.GetSpecificSelectAsync(
            filter: x => !branchId.HasValue || x.BranchId == branchId,
            select: x => new PaginatedCashboxesData
            {
                Id = x.Id,
                Name = x.Name,
                BranchId = x.BranchId,
                BranchName = x.Branch.NameEn,
                IsActive = x.IsActive,
                HasOpenShift = x.Shifts.Any(s => !s.IsClosed && !s.IsDeleted)
            },
            includeProperties: "Branch,Shifts",
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            orderBy: q => q.OrderByDescending(x => x.Id))).ToList();

        return new()
        {
            Data = new CashboxGetAllResponse { Items = items, TotalRecords = total },
            IsSuccess = true
        };
    }

    public async Task<Response<PaginatedCashboxesData>> CreateCashboxAsync(CashboxCreateRequest model)
    {
        bool branchExists = await _unitOfWork.Branches.ExistAsync(x => x.Id == model.BranchId && x.IsActive);
        if (!branchExists)
        {
            string msg = Localization.CannotBeFound;
            return new() { Error = msg, Message = msg };
        }

        var cashbox = new Cashbox
        {
            BranchId = model.BranchId,
            Name = model.Name,
            IsActive = true
        };

        await _unitOfWork.Cashboxes.CreateAsync(cashbox);
        await _unitOfWork.CompleteAsync();

        var branch = await _unitOfWork.Branches.GetByIdAsync(model.BranchId);

        return new()
        {
            Data = new PaginatedCashboxesData
            {
                Id = cashbox.Id,
                Name = cashbox.Name,
                BranchId = cashbox.BranchId,
                BranchName = branch?.NameEn ?? string.Empty,
                IsActive = cashbox.IsActive,
                HasOpenShift = false
            },
            IsSuccess = true
        };
    }

    public async Task<Response<CashboxShiftGetByIdResponse>> OpenShiftAsync(
        CashboxShiftOpenRequest model, string userId)
    {
        var cashbox = await _unitOfWork.Cashboxes
            .GetFirstOrDefaultAsync(x => x.Id == model.CashboxId && x.IsActive);
        if (cashbox is null)
        {
            string msg = Localization.CannotBeFound;
            return new() { Error = msg, Message = msg };
        }

        // Only one open shift per cashbox
        bool hasOpenShift = await _unitOfWork.CashboxShifts
            .ExistAsync(x => x.CashboxId == model.CashboxId && !x.IsClosed);
        if (hasOpenShift)
        {
            const string msg = "A shift is already open for this cashbox.";
            return new() { Error = msg, Message = msg };
        }

        var shift = new CashboxShift
        {
            CashboxId = model.CashboxId,
            OpenedByUserId = userId,
            OpenedAt = DateTime.UtcNow,
            OpeningCash = model.OpeningCash,
            IsClosed = false,
            ExpectedCash = model.OpeningCash,
            Difference = 0,
            IsActive = true
        };

        await _unitOfWork.CashboxShifts.CreateAsync(shift);
        await _unitOfWork.CompleteAsync();

        return new()
        {
            Data = MapShift(shift, cashbox.Name, new List<CashMovementDto>()),
            IsSuccess = true
        };
    }

    public async Task<Response<CashboxShiftGetByIdResponse>> CloseShiftAsync(CashboxShiftCloseRequest model)
    {
        var shift = await _unitOfWork.CashboxShifts
            .GetFirstOrDefaultAsync(x => x.Id == model.ShiftId && !x.IsClosed,
                includeProperties: "Cashbox,Movements");
        if (shift is null)
        {
            const string msg = "Open shift not found.";
            return new() { Error = msg, Message = msg };
        }

        var movements = shift.Movements?.ToList() ?? new List<CashMovement>();
        decimal totalIn = movements.Where(m => m.Type == CashMovementType.CashIn).Sum(m => m.Amount);
        decimal totalOut = movements.Where(m => m.Type == CashMovementType.CashOut).Sum(m => m.Amount);

        shift.ExpectedCash = shift.OpeningCash + totalIn - totalOut;
        shift.ClosingCash = model.ClosingCash;
        shift.Difference = model.ClosingCash - shift.ExpectedCash;
        shift.ClosedAt = DateTime.UtcNow;
        shift.IsClosed = true;

        _unitOfWork.CashboxShifts.Update(shift);
        await _unitOfWork.CompleteAsync();

        var movementDtos = movements.Select(MapMovement).ToList();

        return new()
        {
            Data = MapShift(shift, shift.Cashbox?.Name ?? string.Empty, movementDtos),
            IsSuccess = true
        };
    }

    public async Task<Response<CashMovementDto>> AddMovementAsync(
        CashMovementCreateRequest model, string userId)
    {
        var shift = await _unitOfWork.CashboxShifts
            .GetFirstOrDefaultAsync(x => x.Id == model.ShiftId && !x.IsClosed);
        if (shift is null)
        {
            const string msg = "Open shift not found.";
            return new() { Error = msg, Message = msg };
        }

        var movement = new CashMovement
        {
            CashboxShiftId = model.ShiftId,
            Type = model.Type,
            Amount = model.Amount,
            Reason = model.Reason,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = userId,
            IsActive = true
        };

        await _unitOfWork.CashMovements.CreateAsync(movement);
        await _unitOfWork.CompleteAsync();

        return new() { Data = MapMovement(movement), IsSuccess = true };
    }

    public async Task<Response<CashboxShiftGetByIdResponse>> GetShiftByIdAsync(int shiftId)
    {
        var shift = await _unitOfWork.CashboxShifts
            .GetFirstOrDefaultAsync(x => x.Id == shiftId,
                includeProperties: "Cashbox,Movements");
        if (shift is null)
        {
            string msg = Localization.CannotBeFound;
            return new() { Error = msg, Message = msg, Data = null! };
        }

        var movementDtos = (shift.Movements ?? new List<CashMovement>()).Select(MapMovement).ToList();
        return new()
        {
            Data = MapShift(shift, shift.Cashbox?.Name ?? string.Empty, movementDtos),
            IsSuccess = true
        };
    }

    public async Task<Response<CashLedgerResponse>> GetCashLedgerAsync(
        int branchId, DateTime from, DateTime to)
    {
        // Get all cashboxes for branch
        var cashboxes = (await _unitOfWork.Cashboxes.GetAllAsync(
            x => x.BranchId == branchId)).ToList();

        if (!cashboxes.Any())
        {
            return new()
            {
                Data = new CashLedgerResponse { BranchId = branchId, From = from, To = to },
                IsSuccess = true
            };
        }

        var cashboxIds = cashboxes.Select(c => c.Id).ToList();

        // Get all shifts for those cashboxes
        var allShifts = (await _unitOfWork.CashboxShifts.GetAllAsync(
            x => cashboxIds.Contains(x.CashboxId),
            includeProperties: "Movements")).ToList();

        // Opening balance = movements before 'from'
        var beforeMovements = allShifts
            .SelectMany(s => s.Movements ?? new List<CashMovement>())
            .Where(m => m.CreatedAt < from)
            .ToList();

        decimal openingBalance = beforeMovements.Sum(m =>
            m.Type == CashMovementType.CashIn ? m.Amount : -m.Amount);

        // Period movements
        var periodMovements = allShifts
            .SelectMany(s => s.Movements ?? new List<CashMovement>())
            .Where(m => m.CreatedAt >= from && m.CreatedAt <= to)
            .OrderBy(m => m.CreatedAt)
            .ToList();

        var lines = new List<CashLedgerLineDto>();
        decimal runningBalance = openingBalance;

        foreach (var m in periodMovements)
        {
            if (m.Type == CashMovementType.CashIn)
                runningBalance += m.Amount;
            else
                runningBalance -= m.Amount;

            lines.Add(new CashLedgerLineDto
            {
                Date = m.CreatedAt,
                ShiftId = m.CashboxShiftId,
                Type = m.Type,
                Amount = m.Amount,
                Reason = m.Reason,
                ReferenceType = m.ReferenceType,
                ReferenceId = m.ReferenceId,
                Balance = runningBalance
            });
        }

        decimal totalIn = periodMovements.Where(m => m.Type == CashMovementType.CashIn).Sum(m => m.Amount);
        decimal totalOut = periodMovements.Where(m => m.Type == CashMovementType.CashOut).Sum(m => m.Amount);

        return new()
        {
            Data = new CashLedgerResponse
            {
                BranchId = branchId,
                From = from,
                To = to,
                OpeningBalance = openingBalance,
                TotalCashIn = totalIn,
                TotalCashOut = totalOut,
                ClosingBalance = openingBalance + totalIn - totalOut,
                Lines = lines
            },
            IsSuccess = true
        };
    }

    // ── Internal helper to find the open shift for a branch cashbox ────────────
    public async Task<CashboxShift?> GetOpenShiftForBranchAsync(int branchId)
    {
        var cashbox = await _unitOfWork.Cashboxes
            .GetFirstOrDefaultAsync(x => x.BranchId == branchId && x.IsActive);
        if (cashbox is null) return null;

        return await _unitOfWork.CashboxShifts
            .GetFirstOrDefaultAsync(x => x.CashboxId == cashbox.Id && !x.IsClosed);
    }

    public async Task<bool> CreateCashMovementAsync(
        int shiftId, CashMovementType type, decimal amount,
        string reason, string userId, string? referenceType, int? referenceId)
    {
        var movement = new CashMovement
        {
            CashboxShiftId = shiftId,
            Type = type,
            Amount = amount,
            Reason = reason,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = userId,
            ReferenceType = referenceType,
            ReferenceId = referenceId,
            IsActive = true
        };
        await _unitOfWork.CashMovements.CreateAsync(movement);
        return true;
    }

    // ── Mappers ─────────────────────────────────────────────────────────────────
    private static CashboxShiftGetByIdResponse MapShift(
        CashboxShift shift, string cashboxName, List<CashMovementDto> movementDtos) => new()
    {
        Id = shift.Id,
        CashboxId = shift.CashboxId,
        CashboxName = cashboxName,
        OpenedByUserId = shift.OpenedByUserId,
        OpenedAt = shift.OpenedAt,
        OpeningCash = shift.OpeningCash,
        ClosedAt = shift.ClosedAt,
        ClosingCash = shift.ClosingCash,
        ExpectedCash = shift.ExpectedCash,
        Difference = shift.Difference,
        IsClosed = shift.IsClosed,
        Movements = movementDtos
    };

    private static CashMovementDto MapMovement(CashMovement m) => new()
    {
        Id = m.Id,
        Type = m.Type,
        Amount = m.Amount,
        Reason = m.Reason,
        CreatedAt = m.CreatedAt,
        ReferenceType = m.ReferenceType,
        ReferenceId = m.ReferenceId
    };
}
