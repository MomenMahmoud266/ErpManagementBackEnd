using System;
using System.Collections.Generic;
using System.Text;
using ErpManagement.Domain.Models.Shared;

namespace ErpManagement.Domain.Models.Shared;

public abstract class TenantEntity : BaseEntity, ITenantEntity
{
    public int TenantId { get; set; }
}