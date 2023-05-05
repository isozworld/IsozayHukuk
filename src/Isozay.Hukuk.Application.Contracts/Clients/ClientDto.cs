using System;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Clients
{
    public class ClientDto : AuditedEntityDto<long>
    {
        public string Name { get; set; }
        public ClientIdentifier ClientIdentifier { get; set; }
        public string IdNumber { get; set; }
        public string? TaxDep { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public ClientType ClientType { get; set; }
        public Guid? TenantId { get; set; }
    }
}
