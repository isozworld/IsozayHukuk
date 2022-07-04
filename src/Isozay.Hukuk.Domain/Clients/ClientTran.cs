using Isozay.Hukuk.Currencies;
using Isozay.Hukuk.Safes;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Clients
{
    public class ClientTran : AuditedAggregateRoot<long>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Fiches.FicheType FicheType { get; set; }
        public long FicheId { get; set; }
        public long ClientId { get; set; }
        public long? SafeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public long CurrencyId { get; set; }
        public decimal TrRate { get; set; }
        public string Description { get; set; }
        public char IO { get; set; }
    }
}
