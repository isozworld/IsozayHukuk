using System;
using Volo.Abp.Application.Dtos;

namespace Isozay.Hukuk.Clients
{
    public class ClientRelationDto : AuditedEntityDto<long>
    {
        public Guid? TenantId { get; set; }
        public long ParentClientId { get; set; }
        public long ChildClientId { get; set; }
        public string Description { get; set; }
    }
}
