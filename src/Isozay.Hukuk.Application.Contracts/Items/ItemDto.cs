using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Items
{
    public  class ItemDto : AuditedEntityDto<long>
    {
        public Guid? TenantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Barkod { get; set; }
    }
}
