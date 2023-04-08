using Isozay.Hukuk.Fiches;
using Isozay.Hukuk.Safes;
using System;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Clients
{
    public class ClientTranDto : AuditedEntityDto<long>
    {
        public Guid? TenantId { get; set; }
        public FicheType FicheType { get; set; }
        public long? FicheId { get; set; }
        public string? FicheNo { get; set; }
        public long ClientId { get; set; }
        public long? SafeId { get; set; }
        public string? SafeName { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public long CurrencyId { get; set; }
        public decimal TrRate { get; set; }
        public string Description { get; set; }
        public char IO { get; set; }
        public decimal Debt { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
    }
}
