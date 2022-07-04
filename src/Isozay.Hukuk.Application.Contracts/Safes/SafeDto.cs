using Isozay.Hukuk.Currencies;
using System;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Safes
{
    public class SafeDto : AuditedEntityDto<long> 
    {
        public Guid? TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CurrencyId { get; set; }
        public CurrencyDto Currency { get; set; }
    }
}
