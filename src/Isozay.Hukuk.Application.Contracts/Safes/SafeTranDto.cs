using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Safes
{
    public class SafeTranDto : AuditedEntityDto<long>
    { 
        public Guid? TenantId { get; set; }
        public SafeDto Safe { get; set; }
        public long SafeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public long CurrencyId { get; set; }
        public CurrencyDto Currency { get; set; }
        public decimal TrRate { get; set; }
        public string Description { get; set; }
        public long? ClientId { get; set; }
        public ClientDto? Client { get; set; }
        public char IO { get; set; }
    }
}
