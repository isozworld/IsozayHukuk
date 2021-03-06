using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Auditing;
using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;


using Microsoft.EntityFrameworkCore;


namespace Isozay.Hukuk.Clients
{
    [Authorize(HukukPermissions.Clients.Default)]
    public class ClientService :
            CrudAppService< //Defines CRUD methods
            Client,
            ClientDto, //Used to show Clients
            long, //Primary key of the Client entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateClientDto>, IClientService//Used to create/update a Client
    {
        public ClientService(IRepository<Client, long> repository)
    : base(repository)
        {
            GetPolicyName = HukukPermissions.Clients.Default;
            GetListPolicyName = HukukPermissions.Clients.Default;
            CreatePolicyName = HukukPermissions.Clients.Create;
            UpdatePolicyName = HukukPermissions.Clients.Edit;
            DeletePolicyName = HukukPermissions.Clients.Create;
        }

        [DisableAuditing]

        public async Task<IReadOnlyList<ClientDto>> Search(string searchText)
        {
            //searchString = searchString.ToLowerInvariant();
            //searchString = searchString == null ? "" : searchString.Trim().ToLowerInvariant();
            Console.WriteLine($"______________________________{searchText}");
            var r = await Repository.GetQueryableAsync();
            var q = from client in r
                    //where EF.Functions.Like(client.Name, $"%{searchString}%")
                    where  client.Name.Contains(searchText)

                    select client;

            q = q.OrderByDescending(x => x.CreationTime).Take(10);
            var results = await AsyncExecuter.ToListAsync(q);
            return ObjectMapper.Map<List<Client>, List<ClientDto>>(results);
        }
    }
}
