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

		private readonly IRepository<FicheInstallment, long> _ficheInstallmentRepository;
		private readonly IRepository<FicheLine, long> _ficheLineRepository;
		private readonly IRepository<ClientTran, long> _clientTranRepository;
        private readonly IRepository<Client, long> _clientRepository;

        public FicheService (IRepository<Fiche, long> repository,
		    IRepository<FicheLine, long> ficheLineRepository,
			IRepository<ClientTran, long> clientTranRepository,
            IRepository<Client, long> clientRepository,
            IRepository<FicheInstallment, long> ficheInstallmentRepository
			)
		: base (repository) {
			_ficheInstallmentRepository = ficheInstallmentRepository;
            _clientTranRepository = clientTranRepository;
			_clientRepository = clientRepository;
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
				.Include (x => x.Currency)

				.Skip (input.SkipCount)
				.Take (input.MaxResultCount);

			var queryResult = await AsyncExecuter.ToListAsync (queryable);

			var Dtos = queryResult.Select (x => {
				var dto = ObjectMapper.Map<Fiche, FicheDto> (x);
				dto.Client = ObjectMapper.Map<Client, ClientDto> (x.Client);
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
			var clientTran = (await _clientTranRepository.GetQueryableAsync()).Where(x => x.FicheId == id);
			await _clientTranRepository.DeleteManyAsync(clientTran);
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
			decimal amount = 0;
			int installmentCount = input.FicheInstallments.Count;
			input.FicheLine.ForEach (x => { x.Item = null; amount += x.FicheAmount; });

			input.FicheInstallments.ForEach(x => {
				x.CurrencyId = input.CurrencyId;
				x.Amount = amount / installmentCount;
                switch (input.FicheType)
                {
                    case FicheType.Buying:
                        x.IO = 'O';
                        break;
                    case FicheType.Selling:
                        x.IO = 'I';
                        break;
                    case FicheType.BuyReturn:
                        x.IO = 'I';
                        break;
                    case FicheType.SalesReturn:
                        x.IO = 'O';
                        break;
                    default:
                        break;
                }
            });

			var myfiche = ObjectMapper.Map<CreateUpdateFicheDto, Fiche> (input);

            var rv = await Repository.InsertAsync (myfiche, true);

            var rvFiche = ObjectMapper.Map<Fiche, FicheDto> (rv);

   //         rvFiche.FicheLine = new List<FicheLineDto> ();

			//rvFiche.FicheInstallments = new List<FicheInstallmentDto>();

   //         rv.FicheLine.ForEach (x => rvFiche.FicheLine.Add (ObjectMapper.Map<FicheLine, FicheLineDto> (x)));

   //         rv.FicheInstallments.ForEach(x => rvFiche.FicheInstallments.Add(ObjectMapper.Map<FicheInstallment, FicheInstallmentDto>(x)));

			//await createClientTrans(rvFiche);

            return rvFiche;
		}

		private async Task createClientTrans(FicheDto c)
		{
            var clientTranList = new List<ClientTran>();

            c.FicheInstallments.ForEach(x =>
            {
                var clientTranDto = new ClientTranDto
                {
                    SafeId = c.SafeId,
                    Amount = x.Amount,
                    ClientId = c.ClientId,
                    CurrencyId = c.CurrencyId,
                    FicheType = c.FicheType,
                    FicheId = c.Id,
                    FicheNo = c.FicheNo,
                    TrRate = 1,
                    Description = c.Description,
                    TransactionDate = x.Date,

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

                var clientTran = ObjectMapper.Map<ClientTranDto, ClientTran>(clientTranDto);
                clientTranList.Add(clientTran);
            });

            await _clientTranRepository.InsertManyAsync(clientTranList);
        }

		public async Task<List<FicheInstallmentDto>> GetFicheInstallmentsAsync(long id)
		{
			var queryable = (await _ficheInstallmentRepository.GetQueryableAsync()).Where(x=>x.FicheId == id).ToList();
			var rv = queryable.Select(x =>
			{
				var dto = ObjectMapper.Map<FicheInstallment, FicheInstallmentDto>(x);
				return dto;
			}).ToList();
			return rv;
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
		public async Task DeleteFicheLine(List<long> f)
		{
			foreach (long a in f)
			{
                await _ficheLineRepository.DeleteAsync(a, true);
            }
		}

        [Authorize(Permissions.HukukPermissions.Fiches.Delete)]
        public async Task DeleteFicheInstallments(List<long> f)
        {
            foreach (long a in f)
            {
                await _ficheInstallmentRepository.DeleteAsync(a, true);
            }
        }

        public async Task UpdateFicheLines(List<FicheLineDto> l)
		{
			foreach (FicheLineDto f in l)
			{
				await _ficheLineRepository.UpdateAsync(ObjectMapper.Map<FicheLineDto,FicheLine>(f));
			}
		}

        public async Task UpdateFicheInstallments(List<FicheInstallmentDto> l)
        {
            foreach (FicheInstallmentDto f in l)
            {
                await _ficheInstallmentRepository.UpdateAsync(ObjectMapper.Map<FicheInstallmentDto, FicheInstallment>(f));
            }
        }

        [Authorize (Permissions.HukukPermissions.Fiches.Edit)]
		public async Task CreateFicheLineAsync(FicheLineDto f)
		{
			f.Item = null;
			f.Currency = null;
			var ficheLine = ObjectMapper.Map<FicheLineDto, FicheLine>(f);
			await _ficheLineRepository.InsertAsync(ficheLine);
		}

        public async Task CreateFicheInstallmentAsync(FicheInstallmentDto f)
        {
            f.Currency = null;
            var ficheInstallment = ObjectMapper.Map<FicheInstallmentDto, FicheInstallment>(f);
            await _ficheInstallmentRepository.InsertAsync(ficheInstallment);
        }

        public async Task CreateFicheInstallmentManyAsync(List<FicheInstallmentDto> f)
        {
            foreach (FicheInstallmentDto a in f)
            {
                await CreateFicheInstallmentAsync(a);
            }
        }

        public async Task CreateFicheLineManyAsync(List<FicheLineDto> f)
		{
			foreach (FicheLineDto a in f)
			{
				await CreateFicheLineAsync(a);
			}
		}

		public async Task<IReadOnlyList<FicheDto>> Search(string searchText, long id)
		{
            var r = await Repository.GetQueryableAsync();
            var q = from fiche in r where fiche.Description.Contains(searchText) select fiche;
            var results = await AsyncExecuter.ToListAsync(q);
            return ObjectMapper.Map<List<Fiche>, List<FicheDto>>(results);
        }

        public async Task<List<FicheInstallmentDto>> FillInstallments(int months, DateTime initial)
		{
			var rv = new List<FicheInstallmentDto>();
			var prevDate = initial;
			for (int i = 0; i < months; i++)
			{
				
				var r = new FicheInstallmentDto { Date = prevDate.AddMonths(1), Description = $"{i+1}. taksit" };
				prevDate = r.Date;
                Console.WriteLine($"---------------------------{prevDate}");
                rv.Add(r);
			}
            Console.WriteLine(rv.Count);
            return rv;
		}


        public async Task<List<FicheInstallmentDto>> GetSortedFicheInstallmentsAsync()
		{
			var clients = ObjectMapper.Map<List<Client>, List<ClientDto>>((await _clientRepository.GetQueryableAsync()).ToList());
			var rv = ObjectMapper.Map<List<FicheInstallment>, List<FicheInstallmentDto>>
				((await _ficheInstallmentRepository.GetQueryableAsync()).ToList());
			//var r = (await _ficheInstallmentRepository.GetQueryableAsync()).OrderBy(x => x.Date).ToList();
			//var rv = ObjectMapper.Map<List<FicheInstallment>, List<FicheInstallmentDto>>(r);

			//rv.ForEach(x => x.ClientName = clients.Where(c => c.Id == x.Fiche.ClientId).First().Name);

			//return rv;
			return rv;

		}
    }
}