using ErpManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static ErpManagement.Domain.Constants.Statics.SDStatic.ApiRoutes;

namespace ErpManagement.WebApi.Services;

public class HttpContextCurrentTenant : ICurrentTenant
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentTenant(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int TenantId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var tid =
                user?.FindFirstValue("tid") ??
                user?.FindFirstValue("http://schemas.microsoft.com/identity/claims/tenantid") ??
                user?.FindFirstValue(SDStatic.RequestClaims.TenantId);

            return int.TryParse(tid, out var tenantId) ? tenantId : 0;
        }
    }
}