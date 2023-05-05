using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Isozay.Hukuk.Clients
{
    public class CreateUpdateClientDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public ClientIdentifier ClientIdentifier { get; set; }

        [Required]
        public string IdNumber { get; set; }

        public string? TaxDep { get; set; }

        public string Mail { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public string Fax { get; set; }

        [Required]
        public string Address { get; set; } = "";

        [Required]
        public ClientType ClientType { get; set; } = ClientType.Undefined;
        public Guid? TenantId { get; set; }
    }
}
