using ErpManagement.Domain.Constants.Enums;
using ErpManagement.Domain.DTOs.Request.Transactions;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;

namespace ErpManagement.Services.IServices.Transactions;

public interface IInventoryPeriodService
{
    Task<Response<InventoryPeriodGetAllResponse>> GetAllAsync(int? branchId);
    Task<Response<InventoryPeriodGetByIdResponse>> GetByIdAsync(int id);
    Task<Response<InventoryPeriodGetByIdResponse>> CreateAsync(InventoryPeriodCreateRequest model);
    Task<Response<InventoryPeriodGetByIdResponse>> AddPhysicalCountAsync(int periodId, PhysicalCountCreateRequest model);
    Task<Response<InventoryPeriodGetByIdResponse>> CloseAsync(int periodId);
    Task<Response<string>> DeleteAsync(int id);
}
