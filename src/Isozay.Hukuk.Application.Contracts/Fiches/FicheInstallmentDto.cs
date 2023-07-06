using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Fiches
{
    public class FicheInstallmentDto : AuditedEntityDto<long>
    {
        public Guid? TenantId { get; set; }
        public long FicheId { get; set; }
        public FicheDto Fiche { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public CurrencyDto Currency { get; set; }
        public long CurrencyId { get; set; }
        public FicheType FicheType { get; set; }
        public char IO { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string ClientName { get; set; }
    }
}

