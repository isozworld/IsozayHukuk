using System;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Currencies
{
    public class CurrencyDto : AuditedEntityDto<long>
    {
        public string Name { get; set; }
    }
}
