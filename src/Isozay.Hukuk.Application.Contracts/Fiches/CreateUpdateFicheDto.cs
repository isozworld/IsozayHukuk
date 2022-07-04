using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Fiches
{
    public  class CreateUpdateFicheDto : AuditedEntityDto<long>
    {
        public Guid? TenantId { get; set; }
        public FicheType FicheType { get; set; } = FicheType.Buying;
        public string FicheNo { get; set; }
        public DateTime FicheDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public long CurrencyId { get; set; } = 1;
        public long ClientId { get; set; }
        public long ClientTranId { get; set; }

    }
}
