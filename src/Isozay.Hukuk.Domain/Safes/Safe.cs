using Isozay.Hukuk.Currencies;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Safes
{
    public class Safe : AuditedAggregateRoot<long>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
