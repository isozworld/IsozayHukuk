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

using Abp.Auditing;
using Volo.Abp;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Uow;
using Isozay.Hukuk.Items;
using Isozay.Hukuk.Safes;

namespace Isozay.Hukuk.Fiches {
	[Authorize (HukukPermissions.Clients.Default)]
	public class FicheService :
			CrudAppService< //Defines CRUD methods
			Fiche,
			FicheDto, //Used to show Clients
			long, //Primary key of the Client entity
			PagedAndSortedResultRequestDto, //Used for paging/sorting
			CreateUpdateFicheDto>, IFicheService//Used to create/update a Client
	{

		private readonly IRepository<FicheLine, long> _ficheLineRepository;
        private readonly ClientService _clientService;
		private readonly IRepository<ClientTran, long> _clientTranRepository;

        public FicheService (IRepository<Fiche, long> repository,
		    IRepository<FicheLine, long> ficheLineRepository, ClientService clientService, IRepository<ClientTran, long> clientTranRepository)
		: base (repository) {
			_clientTranRepository = clientTranRepository;
			_clientService = clientService;
			_ficheLineRepository = ficheLineRepository;
			GetPolicyName = HukukPermissions.Fiches.Default;
			GetListPolicyName = HukukPermissions.Fiches.Default;
			CreatePolicyName = HukukPermissions.Fiches.Create;
			UpdatePolicyName = HukukPermissions.Fiches.Edit;
			DeletePolicyName = HukukPermissions.Fiches.Create;
		}

		public override async Task<PagedResultDto<FicheDto>> GetListAsync (
			PagedAndSortedResultRequestDto input) {

			var queryable = await Repository.GetQueryableAsync ();

			queryable = queryable
				.Include (x => x.Client)
				.Include (x => x.FicheLine).ThenInclude (y => y.Item)
				.Include (x => x.ClientTran)
				.Include (x => x.Currency)

				.Skip (input.SkipCount)
				.Take (input.MaxResultCount);

			var queryResult = await AsyncExecuter.ToListAsync (queryable);

			var Dtos = queryResult.Select (x => {
				var dto = ObjectMapper.Map<Fiche, FicheDto> (x);
				dto.Client = ObjectMapper.Map<Client, ClientDto> (x.Client);
				dto.ClientTran = ObjectMapper.Map<ClientTran, ClientTranDto> (x.ClientTran);
				dto.FicheLine = ObjectMapper.Map<List<FicheLine>, List<FicheLineDto>> (x.FicheLine);
				return dto;
			}).ToList ();

			var totalCount = await Repository.GetCountAsync ();

			return new PagedResultDto<FicheDto> (
				totalCount,
				Dtos
			);
		}

        [Authorize(HukukPermissions.Fiches.Edit)]
        public async Task UpdateClientTranAsync(long ficheId)
        {
			var fiche = (await Repository.GetListAsync()).Where(x => x.Id == ficheId).First();
            var ficheLine = await _ficheLineRepository.GetQueryableAsync();
            var ficheLineList = ficheLine.Where(x => x.FicheId == ficheId).ToList();
            decimal newVal = 0;
            ficheLineList.ForEach(x => { newVal += x.UnitPrice * x.FicheQuantity; });
            var tran = await _clientTranRepository.GetQueryableAsync();
            var toUpdate = tran.Where(x => x.FicheId == ficheId).First();

            toUpdate.Amount = newVal;
			toUpdate.ClientId = fiche.ClientId;
			toUpdate.Description = fiche.Description;
			toUpdate.CurrencyId = fiche.CurrencyId;
			toUpdate.TransactionDate = fiche.FicheDate;
			toUpdate.FicheType = fiche.FicheType;

            switch (toUpdate.FicheType)
            {
                case FicheType.Buying:
                    toUpdate.IO = 'O';
                    break;
                case FicheType.Selling:
                    toUpdate.IO = 'I';
                    break;
                case FicheType.BuyReturn:
                    toUpdate.IO = 'I';
                    break;
                case FicheType.SalesReturn:
                    toUpdate.IO = 'O';
                    break;
                default:
                    break;
            }

            await _clientTranRepository.UpdateAsync(toUpdate, true);
        }

		[Authorize(HukukPermissions.Fiches.Delete)]
		public override async Task DeleteAsync(long id)
		{
			var clientTran = (await _clientTranRepository.GetQueryableAsync()).Where(x => x.FicheId == id).First();
			await _clientTranRepository.DeleteAsync(clientTran);
			await base.DeleteAsync(id);
		}

		public async Task<bool> HasAnyFiches(long id)
		{
			var a = (await Repository.GetListAsync()).Where(x => x.ClientId == id);
			return a.Any();
		}


		public async Task<string> getFicheNumber(long? id)
		{
			if (id == null) return "";
			else return (await Repository.GetAsync(id ?? default)).FicheNo;
		}


        [Authorize (HukukPermissions.Fiches.Create)]
		public override async Task<FicheDto> CreateAsync (CreateUpdateFicheDto input) { 
			input.FicheLine.ForEach (x => { x.Item = null; });

			var myfiche = ObjectMapper.Map<CreateUpdateFicheDto, Fiche> (input);

            var rv = await Repository.InsertAsync (myfiche, true);

            var rvFiche = ObjectMapper.Map<Fiche, FicheDto> (rv);

            rvFiche.FicheLine = new List<FicheLineDto> ();

            rv.FicheLine.ForEach (x => { rvFiche.FicheLine.Add (ObjectMapper.Map<FicheLine, FicheLineDto> (x)); });

            await _clientService.CreateClientTran(rvFiche);

            return rvFiche;
		}

		[Authorize (HukukPermissions.Fiches.Edit)]
		public async Task<List<FicheLineDto>> GetListFichLineAsync(long FicheId) {

			var queryable = await _ficheLineRepository.GetQueryableAsync ();

			queryable = queryable
				.Include (x => x.Item)
				.Include (x => x.Currency)
				.Where (x => x.FicheId == FicheId);

			var queryResult = await AsyncExecuter.ToListAsync (queryable);

			var Dtos = queryResult.Select (x => {
				var dto = ObjectMapper.Map<FicheLine, FicheLineDto> (x);
				return dto;

			}).ToList ();

			return Dtos;
		}

		[Authorize (Permissions.HukukPermissions.Fiches.Delete)]
		public async Task DeleteFicheLine(long FicheLineId)
		{
			await _ficheLineRepository.DeleteAsync(FicheLineId, true);
		}


		[Authorize (Permissions.HukukPermissions.Fiches.Edit)]
		public async Task<Task> CreateFicheLineAsync(FicheLineDto f)
		{
			f.Item = null;
			f.Currency = null;
			var ficheLine = ObjectMapper.Map<FicheLineDto, FicheLine>(f);
			await _ficheLineRepository.InsertAsync(ficheLine);
			return Task.CompletedTask;
		}

		public async Task<IReadOnlyList<FicheDto>> Search(string searchText, long id)
		{
            var r = await Repository.GetQueryableAsync();
            var q = from fiche in r where fiche.Description.Contains(searchText) where fiche.ClientId == id select fiche;
            var results = await AsyncExecuter.ToListAsync(q);
            return ObjectMapper.Map<List<Fiche>, List<FicheDto>>(results);
        }
    }
}