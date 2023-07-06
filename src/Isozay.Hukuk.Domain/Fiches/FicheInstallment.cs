using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Fiches
{
    public class FicheInstallment : AuditedAggregateRoot<long>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public long FicheId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public long CurrencyId { get; set; }
        public FicheType FicheType { get; set; }
        public char IO { get; set; }
        public DateTime Date { get; set; }
    }
}

