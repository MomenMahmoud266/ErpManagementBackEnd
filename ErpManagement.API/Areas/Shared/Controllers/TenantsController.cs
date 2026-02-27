using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Constants.Statics;
using ErpManagement.Domain.DTOs.Response.Core;
using ErpManagement.Domain.Interfaces;

namespace ErpManagement.API.Areas.Shared.Controllers;

[Area(Modules.Shared)]
[ApiExplorerSettings(GroupName = Modules.Shared)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TenantsController(ErpManagementDbContext db, ICurrentTenant currentTenant) : ControllerBase
{
    private readonly ErpManagementDbContext _db = db;
    private readonly ICurrentTenant _currentTenant = currentTenant;

    [HttpGet(ApiRoutes.Tenant.Me)]
    [Produces(typeof(Response<TenantMeResponse>))]
    public async Task<IActionResult> Me()
    {
        var tenantId = _currentTenant.TenantId;
        if (tenantId <= 0)
        {
            return Unauthorized(new Response<TenantMeResponse>
            {
                IsSuccess = false,
                Message = "Unauthorized",
                Error = "No valid tenant context found."
            });
        }

        var tenant = await _db.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tenantId && t.IsActive);

        if (tenant is null)
        {
            return NotFound(new Response<TenantMeResponse>
            {
                IsSuccess = false,
                Message = "Not Found",
                Error = "Tenant not found or inactive."
            });
        }

        var response = new Response<TenantMeResponse>
        {
            IsSuccess = true,
            Message = "Success",
            Data = new TenantMeResponse
            {
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                BusinessType = tenant.BusinessType.ToString(),
                EnableInventory = tenant.EnableInventory,
                EnableAppointments = tenant.EnableAppointments,
                EnableMemberships = tenant.EnableMemberships,
                EnableTables = tenant.EnableTables,
                EnableKitchenRouting = tenant.EnableKitchenRouting,
                TrialEndsAt = tenant.TrialEndsAt,
                SubscriptionEndsAt = tenant.SubscriptionEndsAt,
                IsSubscriptionActive = tenant.IsSubscriptionActive,
                IsExpired = !tenant.IsAccessAllowed,
                CurrencyCode = tenant.CurrencyCode,
                CountryCode = tenant.CountryCode,
                TimeZoneId = tenant.TimeZoneId,
                TaxLabel = tenant.TaxLabel,
                InventoryMode = tenant.InventoryMode,
                CostingMethod = tenant.CostingMethod
            }
        };

        return Ok(response);
    }
}
