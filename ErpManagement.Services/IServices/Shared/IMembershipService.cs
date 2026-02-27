using ErpManagement.Domain.DTOs.Request;
using ErpManagement.Domain.DTOs.Request.Shared.Gym;
using ErpManagement.Domain.DTOs.Response.Shared.Gym;
using ErpManagement.Domain.DTOs.Response.Transactions;
using ErpManagement.Domain.Dtos.Response;

namespace ErpManagement.Services.IServices.Shared;

public interface IMembershipService
{
    Task<Response<MembershipPlanGetAllResponse>> GetAllPlansAsync(RequestLangEnum lang, PaginationRequest model, int? branchId);
    Task<Response<MembershipPlanCreateRequest>> CreatePlanAsync(MembershipPlanCreateRequest model);
    Task<Response<MembershipPlanUpdateRequest>> UpdatePlanAsync(int id, MembershipPlanUpdateRequest model);
    Task<Response<object>> DeletePlanAsync(int id);
    Task<Response<SaleGetByIdResponse>> PurchaseMembershipAsync(PurchaseMembershipRequest model);
    Task<Response<CheckInResponse>> CheckInAsync(CheckInRequest model);
}
