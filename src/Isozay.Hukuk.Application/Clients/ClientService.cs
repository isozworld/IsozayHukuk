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
using Isozay.Hukuk.Fiches;
using Isozay.Hukuk.Safes;



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
        public readonly IRepository<ClientTran, long> _clientTranRepository;
        public ClientService(IRepository<Client, long> repository, IRepository<ClientTran, long> clientTranRepository)
		: base(repository)
		{
			_clientTranRepository = clientTranRepository;
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

		public async Task<ClientTranDto> CreateClientTran (FicheDto c, List<FicheLineDto> l)
		{

			var clientTranDto = new ClientTranDto();

			clientTranDto.Amount = 0;
			clientTranDto.ClientId = c.ClientId;
			clientTranDto.CurrencyId = c.CurrencyId;
			clientTranDto.FicheType = c.FicheType;
			clientTranDto.FicheId = c.Id;
			clientTranDto.TrRate = 1;
			switch (c.FicheType)
			{
				case FicheType.Buying:
					clientTranDto.IO = 'O';
                    break;
                case FicheType.Selling:
                    clientTranDto.IO = 'I';
                    break;
                case FicheType.BuyReturn:
                    clientTranDto.IO = 'I';
                    break;
                case FicheType.SalesReturn:
                    clientTranDto.IO = 'O';
                    break;
                default:
					break;
			}

			l.ForEach(x => { clientTranDto.Amount += x.UnitPrice * x.FicheQuantity; });

			var clientTran = ObjectMapper.Map<ClientTranDto, ClientTran>(clientTranDto);
			await _clientTranRepository.InsertAsync(clientTran);
			var rv = ObjectMapper.Map<ClientTran, ClientTranDto>(clientTran);
			return rv;
		}

		public async Task<List<ClientTranDto>> GetClientTranDtoHistory(long id)
		{

			Console.WriteLine($"______________________ {id}");
			var queryable = await _clientTranRepository.GetQueryableAsync();
            queryable = queryable.Where(x => x.ClientId == id);

            var queryRequest = await AsyncExecuter.ToListAsync(queryable);

            var Dtos = queryRequest.Select(x =>
            {
                var dto = ObjectMapper.Map<ClientTran, ClientTranDto>(x);
                return dto;
            }).ToList();

            return Dtos;
        }

        public async Task<ClientTranDto> CreateClientTran(CreateUpdateSafeTranDto c, long ficheId)
		{
            var clientTranDto = new ClientTranDto();
            Console.WriteLine("--------------------------GATOARABE1");

            clientTranDto.Amount = c.Amount;
            clientTranDto.ClientId = c.ClientId ?? default(long);
            clientTranDto.CurrencyId = c.CurrencyId;
            clientTranDto.SafeId = c.SafeId;
			clientTranDto.FicheId = ficheId;
            clientTranDto.TrRate = 1;
			clientTranDto.IO = 'O';
            Console.WriteLine("--------------------------GATOARABE2");


            var clientTran = ObjectMapper.Map<ClientTranDto, ClientTran>(clientTranDto);
            await _clientTranRepository.InsertAsync(clientTran);
            var rv = ObjectMapper.Map<ClientTran, ClientTranDto>(clientTran);
            return rv;
        }

    }
}
