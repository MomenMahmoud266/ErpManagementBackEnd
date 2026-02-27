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

    public int TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ErpManagement.Domain.Constants.Statics.SDStatic.RequestClaims.TenantId)?.Value;
            return int.TryParse(claim, out var tid) && tid > 0 ? tid : 0;
        }
    }
}