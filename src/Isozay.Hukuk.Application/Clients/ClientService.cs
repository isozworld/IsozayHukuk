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
	    Console.WriteLine($"______________________________{searchText}");
	    var r = await Repository.GetQueryableAsync();
	    var q = from client in r
		    where  client.Name.Contains(searchText)
		    select client;
	    var results = await AsyncExecuter.ToListAsync(q);
	    return ObjectMapper.Map<List<Client>, List<ClientDto>>(results);
	}

	public async Task<IReadOnlyList<ClientDto>> GetListSearchAsync(string searchText)
	{
	    var r = await Repository.GetQueryableAsync();
	    if (!String.IsNullOrEmpty(searchText)) r = r.Where(x => x.Name.Contains(searchText));
	    var results = await AsyncExecuter.ToListAsync(r);
	    return ObjectMapper.Map<List<Client>, List<ClientDto>>(results);
	}
    }
}
