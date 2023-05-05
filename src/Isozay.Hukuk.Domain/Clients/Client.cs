using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Clients
{
    public class Client : AuditedAggregateRoot<long> , IMultiTenant
    {
        public string Name { get; set; }
        public ClientIdentifier ClientIdentifier { get; set; }
        public string IdNumber { get; set; }
        public string? TaxDep { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public ClientType ClientType { get; set; }
        public Guid? TenantId { get; set; }
    }
}
