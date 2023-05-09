using System;
using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Currencies;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Safes
{
	public class CreateUpdateSafeTranDto : AuditedEntityDto<long> {

		public Guid? TenantId { get; set; }
		public long SafeId { get; set; }
		public DateTime TransactionDate { get; set; } = DateTime.Now;
		public decimal Amount { get; set; }
		public long CurrencyId { get; set; } = 1;
		public decimal TrRate { get; set; }
		public string Description { get; set; }
        public long? FicheId { get; set; }
        public long? ClientId { get; set; }
		public char IO { get; set; }
	}
}

