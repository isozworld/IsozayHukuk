using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Safes
{
    public class SafeTran : AuditedAggregateRoot<long>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Safe Safe { get; set; }
        public long SafeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public long? CurrencyId { get; set; }
        public decimal TrRate { get; set; }
        public string Description { get; set; }
        public Client? Client { get; set; }
        public long? ClientId { get; set; }
        public char IO { get; set; }
    }
}
