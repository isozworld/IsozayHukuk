
using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Fiches
{
    public class FicheLine : AuditedAggregateRoot<long>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public long FicheId { get; set; }
        public decimal FicheQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal FicheAmount { get; set; }
        public decimal Vat { get; set; }
        public decimal VatTotal { get; set; }
        public decimal TrCurr { get; set; }
        public long CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public FicheType FicheType { get; set; }
        public char IO { get; set; }
        public long ItemId { get; set; }
                public Items.Item Item { get; set; }
    }
}
