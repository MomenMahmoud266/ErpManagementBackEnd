using System;
using System.Collections.Generic;
using System.Text;

namespace ErpManagement.Domain.Interfaces;

public interface ICurrentTenant
{
    int TenantId { get; }
}
