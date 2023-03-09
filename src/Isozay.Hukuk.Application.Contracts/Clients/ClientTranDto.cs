using Isozay.Hukuk.Currencies;
using Isozay.Hukuk.Safes;
using Isozay.Hukuk.Fiches;
using System;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Clients
{
    public class ClientTranDto : AuditedEntityDto<long>
    {
        public Guid? TenantId { get; set; }
        public FicheType FicheType { get; set; }
        public long FicheId { get; set; }
        public long ClientId { get; set; }
        public long? SafeId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public long CurrencyId { get; set; }
        public decimal TrRate { get; set; }
        public string Description { get; set; }
        public char IO { get; set; }
    }
}
