using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ErpManagement.Domain.Interfaces;

namespace ErpManagement.WebApi.Services;

public class HttpContextCurrentTenant : ICurrentTenant
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentTenant(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // For Day 2: return fixed tenant (1). Later, replace with JWT claim reading:
    // var tenantClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value;
    // int.TryParse(tenantClaim, out var tid);
    // return tid > 0 ? tid : 1;
    public int TenantId
    {
        get
        {
            // Temporary fixed tenant for early development
            return 1;
        }
    }
}