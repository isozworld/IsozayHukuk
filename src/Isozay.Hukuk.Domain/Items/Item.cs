using Isozay.Hukuk.Currencies;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Items
{
    public  class Item : AuditedAggregateRoot<long>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Barkod { get; set; }
    }
}
