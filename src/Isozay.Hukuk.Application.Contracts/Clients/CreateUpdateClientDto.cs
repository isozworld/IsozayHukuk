using System;
using System.ComponentModel.DataAnnotations;

namespace Isozay.Hukuk.Clients
{
    public class CreateUpdateClientDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        public string TaxDep { get; set; }
        public string TaxNumber { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; } = "";
        public ClientType ClientType { get; set; } = ClientType.Undefined;
        public Guid? TenantId { get; set; }
    }
}
