using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Fiches
{
    public class FicheLineDto : AuditedEntityDto<long>
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
        public CurrencyDto Currency { get; set; }
        public FicheType FicheType { get; set; }
        public string Description { get; set; }
        public char IO { get; set; }
        public long ItemId { get; set; }
        public Items.ItemDto Item { get; set; }
    }
}
