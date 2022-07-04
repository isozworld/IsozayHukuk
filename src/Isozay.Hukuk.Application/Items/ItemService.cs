using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Isozay.Hukuk.Items
{
    [Authorize(HukukPermissions.Items.Default)]
    public class ItemsService :
            CrudAppService< //Defines CRUD methods
            Item,
            ItemDto, //Used to show Clients
            long, //Primary key of the Client entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            ItemDto>, IItemService//Used to create/update a Client
    {
        public ItemsService(IRepository<Item, long> repository)
    : base(repository)
        {
            GetPolicyName = HukukPermissions.Items.Default;
            GetListPolicyName = HukukPermissions.Items.Default;
            CreatePolicyName = HukukPermissions.Items.Create;
            UpdatePolicyName = HukukPermissions.Items.Edit;
            DeletePolicyName = HukukPermissions.Items.Create;
        }

 
    }
}
