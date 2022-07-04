using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Currencies
{
    public class Currency : AuditedAggregateRoot<long>
    {
        public string Name { get; set; }
    }
}
