using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Isozay.Hukuk.Fiches
{
    public  class Fiche : AuditedAggregateRoot<long>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public FicheType FicheType { get; set; }
        public string FicheNo { get; set; } 
        public DateTime FicheDate { get; set; }
        public string Description { get; set; }
        public Currency Currency { get; set; }
        public long CurrencyId { get;set; }
        public Client Client { get; set; }
        public long ClientId { get; set; }
        public ClientTran ClientTran { get; set; }
        public List<FicheLine> FicheLine { get; set; }




    }
}
