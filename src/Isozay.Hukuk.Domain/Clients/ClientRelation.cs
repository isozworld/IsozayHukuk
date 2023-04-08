using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Clients
{
    public class ClientRelation : AuditedAggregateRoot<long>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public long ParentClientId { get; set; }
        public long ChildClientId { get; set; }
        public string Description { get; set; }
    }
}