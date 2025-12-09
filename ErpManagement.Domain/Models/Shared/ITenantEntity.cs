using System;
using System.Collections.Generic;
using System.Text;

namespace ErpManagement.Domain.Models.Shared;

public interface ITenantEntity
{
    int TenantId { get; set; }
}