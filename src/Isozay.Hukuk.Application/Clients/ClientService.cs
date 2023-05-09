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
        public readonly IRepository<ClientRelation, long> _clientRelationRepository;
        public readonly IRepository<FicheLine, long> _ficheLineRepository;
        public readonly IRepository<Fiche, long> _ficheRepository;
        public readonly IRepository<Safe, long> _safeRepository;
        public ClientService(IRepository<Client, long> repository,
            IRepository<ClientTran, long> clientTranRepository,
            IRepository<ClientRelation, long> clientRelationRepository,
            IRepository<Fiche,long> ficheRepository,
            IRepository<FicheLine, long> ficheLineRepository,
            IRepository<Safe, long> safeRepository)
		: base(repository)
		{
            _ficheRepository = ficheRepository;
            _safeRepository = safeRepository;
            _ficheLineRepository = ficheLineRepository;
            _clientRelationRepository = clientRelationRepository;
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

        public async Task<bool> isUsed(string s)
        {
            var r = (await Repository.GetQueryableAsync()).ToList().Where(x => x.IdNumber == s);
            return r.Any();
        }

        [Authorize(HukukPermissions.Clients.Create)]
        public async Task<ClientTranDto> CreateClientTran (FicheDto c)
		{
            var safes = (await _safeRepository.GetQueryableAsync()).Where(x => x.SafeType == SafeType.Bureaue);

            var clientTranDto = new ClientTranDto
            {
                SafeId = safes.First().Id,
                Amount = 0,
                ClientId = c.ClientId,
                CurrencyId = c.CurrencyId,
                FicheType = c.FicheType,
                FicheId = c.Id,
                TrRate = 1,
                Description = c.Description
            };

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

			c.FicheLine.ForEach(x => { clientTranDto.Amount += x.UnitPrice * x.FicheQuantity; });

			var clientTran = ObjectMapper.Map<ClientTranDto, ClientTran>(clientTranDto);
			await _clientTranRepository.InsertAsync(clientTran);
			var rv = ObjectMapper.Map<ClientTran, ClientTranDto>(clientTran);
			return rv;
		}

        [Authorize(HukukPermissions.Clients.Create)]
        public async Task<ClientRelationDto> CreateClientRelation(CreateUpdateClientDto c, long ParentId, string desc)
        {
            var child = await CreateAsync(c);
            var relationDto = new ClientRelationDto
            {
                ParentClientId = ParentId,
                ChildClientId = child.Id,
                Description = desc
            };
            var relation = ObjectMapper.Map<ClientRelationDto, ClientRelation>(relationDto);
            var rev = await _clientRelationRepository.InsertAsync(relation);
            var revDto = ObjectMapper.Map<ClientRelation, ClientRelationDto>(rev);
            return revDto;
        }

        public override async Task DeleteAsync(long id)
        {
            var toDel = (await _clientRelationRepository.GetQueryableAsync()).Where(x => x.ChildClientId == id || x.ParentClientId == id);
            await _clientRelationRepository.DeleteManyAsync(toDel);
            await base.DeleteAsync(id);
        }

        public async Task<ClientTranDto> CreateClientTran(SafeTranDto c, char p)
        {
            var clientTranDto = new ClientTranDto
            {
                FicheNo = c.Fiche.FicheNo,
                Amount = c.Amount,
                Description = c.Description,
                ClientId = c.ClientId ?? 0,
                CurrencyId = c.CurrencyId,
                SafeId = c.SafeId,
                TrRate = 1,
                IO = p,
            };

            var clientTran = ObjectMapper.Map<ClientTranDto, ClientTran>(clientTranDto);
            await _clientTranRepository.InsertAsync(clientTran);
            var rv = ObjectMapper.Map<ClientTran, ClientTranDto>(clientTran);
            return rv;
        }

        public async Task<ClientRelationDto> getParentRelation(long id)
        {
            var rv = (await _clientRelationRepository.GetQueryableAsync()).Where(x => x.ChildClientId == id);
            if (rv.Any())
            {
                return ObjectMapper.Map<ClientRelation, ClientRelationDto>(rv.First());
            }
            else return null;
        }

        public async Task UpdateClientAndRelation(CreateUpdateClientDto c, long childId, long newParentId, string newDescription, bool hadParentBefore)
        {
            await base.UpdateAsync(childId,c);
            if (hadParentBefore)
            {
                var t = (await _clientRelationRepository.GetQueryableAsync()).Where(x => x.ChildClientId == childId).First();
                t.ParentClientId = newParentId;
                t.Description = newDescription;
                await _clientRelationRepository.UpdateAsync(t);
            }
            else
            {
                var t = new ClientRelationDto()
                {
                    ParentClientId = newParentId,
                    ChildClientId = childId,
                    Description = newDescription
                };
                await _clientRelationRepository.InsertAsync(ObjectMapper.Map<ClientRelationDto, ClientRelation>(t));
            }
        }

        public async Task<List<ClientTranDto>> GetClientTranDtoHistory(long id, bool childIncluded, string selectedSafeName = "Tümü")
		{
            var queryable = await _clientTranRepository.GetQueryableAsync();

            if (childIncluded)
            {
                var relat = await _clientRelationRepository.GetQueryableAsync();
                var childList = relat.Where(x => x.ParentClientId == id).ToList();
                var childIds = new List<long>();
                foreach (ClientRelation c in childList) childIds.Add(c.ChildClientId);
                queryable = queryable.Where(x => x.ClientId == id || childIds.Contains(x.ClientId));
            }

            else
            {
                queryable = queryable.Where(x => x.ClientId == id);
            }

            var queryRequest = await AsyncExecuter.ToListAsync(queryable);

            var Dtos = queryRequest.Select(x =>
            {
                var dto = ObjectMapper.Map<ClientTran, ClientTranDto>(x);
                return dto;
            }).ToList();

            foreach (ClientTranDto current in Dtos)
            {
                var fiches = await _ficheRepository.GetQueryableAsync();
                var safes = await _safeRepository.GetQueryableAsync();

                if (current.FicheId != null) current.FicheNo = fiches.Where(x => x.Id == current.FicheId).First().FicheNo;
                current.SafeName = safes.Where(x => x.Id == current.SafeId).First().Name;
            }

            if (selectedSafeName != "Tümü")
                Dtos = Dtos.Where(x => x.SafeName == selectedSafeName).ToList();

            ClientTranDto previous = null;

            foreach (ClientTranDto current in Dtos)
            {

                if (current.IO == 'I')
                {
                    current.Debt = current.Amount;
                    current.Credit = 0;
                }

                else
                {
                    current.Debt = 0;
                    current.Credit = current.Amount;
                }

                current.Balance = current.Credit - current.Debt;
                if (previous != null) current.Balance += previous.Balance;

                previous = current;
            }

            return Dtos;
        }
    }
}
