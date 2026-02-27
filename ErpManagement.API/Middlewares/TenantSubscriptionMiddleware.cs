using System.Security.Claims;
using ErpManagement.DataAccess.DbContext;
using ErpManagement.Domain.Constants.Statics;
using ErpManagement.Domain.Dtos.Response;
using Microsoft.EntityFrameworkCore;

namespace ErpManagement.API.Middlewares;

/// <summary>
/// Enforces tenant subscription / trial gating on every authenticated request.
/// Unauthenticated requests are passed through (auth middleware handles them).
/// Allowlisted paths (auth, tenants/me, demo, swagger) always pass through.
/// </summary>
public class TenantSubscriptionMiddleware
{
    private readonly RequestDelegate _next;

    // Paths that must remain accessible even when a subscription is expired.
    private static readonly string[] _allowedPrefixes =
    [
        "/api/authentications/",   // login / logout
        "/api/tenants/me",         // tenant status endpoint
        "/api/demo",               // demo seed (admin only, but allowed for setup)
        "/swagger",                // Swagger UI + JSON
        "/health"                  // optional health-check
    ];

    public TenantSubscriptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ErpManagementDbContext db)
    {
        // 1. Not authenticated → pass through (let [Authorize] handle it)
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        // 2. Allowlisted paths always pass through
        var path = context.Request.Path.Value ?? string.Empty;
        if (IsAllowlisted(path))
        {
            await _next(context);
            return;
        }

        // 3. Resolve tenantId from JWT claim
        var tidClaim = context.User.FindFirst(SDStatic.RequestClaims.TenantId)?.Value;
        if (!int.TryParse(tidClaim, out var tenantId) || tenantId <= 0)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await WriteJson(context, 403, "No valid tenant context.");
            return;
        }

        // 4. Load tenant (read-only)
        var tenant = await db.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tenantId);

        if (tenant is null || !tenant.IsActive)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await WriteJson(context, 403, "Tenant not found or inactive.");
            return;
        }

        // 5. Subscription / trial gate
        if (!tenant.IsAccessAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status402PaymentRequired;
            await WriteJson(context, 402, "Subscription expired. Please renew your subscription.");
            return;
        }

        await _next(context);
    }

    private static bool IsAllowlisted(string path)
    {
        var lower = path.ToLowerInvariant();
        foreach (var prefix in _allowedPrefixes)
        {
            if (lower.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private static async Task WriteJson(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        var body = new
        {
            IsSuccess = false,
            Message = message,
            Error = message,
            StatusCode = statusCode
        };
        await context.Response.WriteAsJsonAsync(body);
    }
}
