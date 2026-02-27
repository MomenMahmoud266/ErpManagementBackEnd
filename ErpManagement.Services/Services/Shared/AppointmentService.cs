using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Shared.Appointment;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Shared.Appointment;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;
using ErpManagement.Domain.Interfaces;
using ErpManagement.Domain.Models.Clinic;
using ErpManagement.Services.IServices.Shared;
using ErpManagement.Services.IServices.Transactions;
using ErpManagement.Services.IServices.WebSocket;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using static ErpManagement.Domain.Constants.Statics.SDStatic;

namespace ErpManagement.Services.Services.Shared;

public class AppointmentService(
    IUnitOfWork unitOfWork,
    IStringLocalizer<SharedResource> sharLocalizer,
    IHubContext<BroadcastHub, IHubClient> hubContext,
    ISaleService saleService,
    ICurrentTenant currentTenant) : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext = hubContext;
    private readonly ISaleService _saleService = saleService;
    private readonly ICurrentTenant _currentTenant = currentTenant;

    public async Task<Response<AppointmentGetAllResponse>> GetAllAsync(RequestLangEnum lang, AppointmentGetAllFiltrationsRequest model)
    {
        var totalRecords = await _unitOfWork.Appointments.CountAsync(x =>
            (!model.BranchId.HasValue || x.BranchId == model.BranchId) &&
            (string.IsNullOrEmpty(model.StaffUserId) || x.StaffUserId == model.StaffUserId) &&
            (!model.From.HasValue || x.StartAt >= model.From) &&
            (!model.To.HasValue || x.StartAt <= model.To) &&
            (!model.Status.HasValue || x.Status == model.Status));

        var items = (await _unitOfWork.Appointments.GetSpecificSelectAsync(
            filter: x =>
                (!model.BranchId.HasValue || x.BranchId == model.BranchId) &&
                (string.IsNullOrEmpty(model.StaffUserId) || x.StaffUserId == model.StaffUserId) &&
                (!model.From.HasValue || x.StartAt >= model.From) &&
                (!model.To.HasValue || x.StartAt <= model.To) &&
                (!model.Status.HasValue || x.Status == model.Status),
            select: x => new PaginatedAppointmentsData
            {
                Id = x.Id,
                BranchId = x.BranchId,
                CustomerId = x.CustomerId,
                CustomerName = x.Customer.FirstName + (x.Customer.LastName != null ? " " + x.Customer.LastName : ""),
                StaffUserId = x.StaffUserId,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                Status = x.Status,
                SaleId = x.SaleId
            },
            includeProperties: "Customer",
            pageNumber: model.PageNumber,
            pageSize: model.PageSize,
            orderBy: q => q.OrderByDescending(x => x.Id)
        )).ToList();

        var result = new AppointmentGetAllResponse
        {
            TotalRecords = totalRecords,
            Items = items
        };

        return new() { Data = result, IsSuccess = true };
    }

    public async Task<Response<AppointmentCreateRequest>> CreateAsync(AppointmentCreateRequest model)
    {
        bool branchExists = await _unitOfWork.Branches.ExistAsync(x => x.Id == model.BranchId && x.IsActive);
        if (!branchExists)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Branch]);
            return new() { Message = msg, Error = msg };
        }

        bool customerExists = await _unitOfWork.Customer.ExistAsync(x => x.Id == model.CustomerId && x.IsActive);
        if (!customerExists)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.Shared.Customer]);
            return new() { Message = msg, Error = msg };
        }

        bool staffExists = await _unitOfWork.Users.ExistAsync(
            u => u.Id == model.StaffUserId && u.TenantId == _currentTenant.TenantId && u.IsActive);
        if (!staffExists)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.User]);
            return new() { Message = msg, Error = msg };
        }

        if (model.EndAt <= model.StartAt)
        {
            string msg = _sharLocalizer[Localization.NotValidDate];
            return new() { Message = msg, Error = msg };
        }

        var appointment = new Appointment
        {
            BranchId = model.BranchId,
            CustomerId = model.CustomerId,
            StaffUserId = model.StaffUserId,
            StartAt = model.StartAt,
            EndAt = model.EndAt,
            Notes = model.Notes,
            Status = AppointmentStatus.Scheduled
        };

        await _unitOfWork.Appointments.CreateAsync(appointment);
        await _unitOfWork.CompleteAsync();

        if (model.Items.Count > 0)
        {
            var items = model.Items.Select(i => new AppointmentItem
            {
                AppointmentId = appointment.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = (i.Quantity * i.UnitPrice) - i.Discount
            }).ToList();

            await _unitOfWork.AppointmentItems.CreateRangeAsync(items);
            await _unitOfWork.CompleteAsync();
        }

        await _hubContext.Clients.All.BroadcastMessage();
        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Done] };
    }

    public async Task<Response<AppointmentGetByIdResponse>> GetByIdAsync(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetFirstOrDefaultAsync(
            x => x.Id == id,
            includeProperties: "Customer,Items,Items.Product");

        if (appointment is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.ClientAppointmentMaking]);
            return new() { Message = msg, Error = msg };
        }

        var response = new AppointmentGetByIdResponse
        {
            Id = appointment.Id,
            BranchId = appointment.BranchId,
            CustomerId = appointment.CustomerId,
            CustomerName = appointment.Customer.FirstName + (appointment.Customer.LastName != null ? " " + appointment.Customer.LastName : ""),
            StaffUserId = appointment.StaffUserId,
            StartAt = appointment.StartAt,
            EndAt = appointment.EndAt,
            Status = appointment.Status,
            Notes = appointment.Notes,
            SaleId = appointment.SaleId,
            Items = appointment.Items.Select(i => new AppointmentItemDto
            {
                ProductId = i.ProductId,
                ProductTitle = i.Product.Title,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = i.TotalAmount
            }).ToList()
        };

        return new() { Data = response, IsSuccess = true };
    }

    public async Task<Response<AppointmentUpdateRequest>> UpdateAsync(int id, AppointmentUpdateRequest model)
    {
        if (id != model.Id)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Message = msg, Error = msg };
        }

        var appointment = await _unitOfWork.Appointments.GetFirstOrDefaultAsync(
            x => x.Id == id,
            includeProperties: "Items");

        if (appointment is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.ClientAppointmentMaking]);
            return new() { Message = msg, Error = msg };
        }

        if (model.EndAt <= model.StartAt)
        {
            string msg = _sharLocalizer[Localization.NotValidDate];
            return new() { Message = msg, Error = msg };
        }

        appointment.BranchId = model.BranchId;
        appointment.CustomerId = model.CustomerId;
        appointment.StaffUserId = model.StaffUserId;
        appointment.StartAt = model.StartAt;
        appointment.EndAt = model.EndAt;
        appointment.Status = model.Status;
        appointment.Notes = model.Notes;

        _unitOfWork.Appointments.Update(appointment);

        // Replace items
        if (appointment.Items.Count > 0)
            _unitOfWork.AppointmentItems.DeleteRange(appointment.Items);

        if (model.Items.Count > 0)
        {
            var newItems = model.Items.Select(i => new AppointmentItem
            {
                AppointmentId = appointment.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalAmount = (i.Quantity * i.UnitPrice) - i.Discount
            }).ToList();

            await _unitOfWork.AppointmentItems.CreateRangeAsync(newItems);
        }

        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { Data = model, IsSuccess = true, Message = _sharLocalizer[Localization.Updated] };
    }

    public async Task<Response<object>> DeleteAsync(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetFirstOrDefaultAsync(
            x => x.Id == id,
            includeProperties: "Items");

        if (appointment is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.ClientAppointmentMaking]);
            return new() { Message = msg, Error = msg };
        }

        if (appointment.Items.Count > 0)
            _unitOfWork.AppointmentItems.DeleteRange(appointment.Items);

        _unitOfWork.Appointments.Delete(appointment);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return new() { IsSuccess = true, Message = _sharLocalizer[Localization.Deleted] };
    }

    public async Task<Response<SaleGetByIdResponse>> CompleteAndInvoiceAsync(int appointmentId, AppointmentCompleteInvoiceRequest model)
    {
        var appointment = await _unitOfWork.Appointments.GetFirstOrDefaultAsync(
            x => x.Id == appointmentId,
            includeProperties: "Items");

        if (appointment is null)
        {
            string msg = string.Format(_sharLocalizer[Localization.CannotBeFound], _sharLocalizer[Localization.ClientAppointmentMaking]);
            return new() { Message = msg, Error = msg };
        }

        if (appointment.Status == AppointmentStatus.Cancelled || appointment.Status == AppointmentStatus.NoShow)
        {
            string msg = _sharLocalizer[Localization.Error];
            return new() { Message = msg, Error = msg };
        }

        if (appointment.SaleId.HasValue)
        {
            string msg = _sharLocalizer[Localization.IsExist];
            return new() { Message = msg, Error = msg };
        }

        var saleRequest = new SaleCreateRequest
        {
            BranchId = appointment.BranchId,
            WarehouseId = model.WarehouseId,
            CustomerId = appointment.CustomerId,
            BillerId = model.BillerId,
            PaidAmount = model.PaidAmount,
            PaymentType = model.PaymentType,
            TransactionNumber = model.TransactionNumber,
            AccountNumber = model.AccountNumber,
            SaleDate = DateTime.UtcNow,
            Items = appointment.Items.Select(i => new SaleItemCreateRequest
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount
            }).ToList()
        };

        var saleResult = await _saleService.CreateAsync(saleRequest);

        if (!saleResult.IsSuccess)
            return new() { Message = saleResult.Message, Error = saleResult.Error };

        appointment.Status = AppointmentStatus.Completed;
        appointment.SaleId = saleResult.Data.Id;
        _unitOfWork.Appointments.Update(appointment);
        await _unitOfWork.CompleteAsync();
        await _hubContext.Clients.All.BroadcastMessage();

        return saleResult;
    }
}
