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

namespace Isozay.Hukuk.Fiches
{
	[Authorize(HukukPermissions.Clients.Default)]
	public class FicheService :
			CrudAppService< //Defines CRUD methods
			Fiche,
			FicheDto, //Used to show Clients
			long, //Primary key of the Client entity
			PagedAndSortedResultRequestDto, //Used for paging/sorting
			CreateUpdateFicheDto>, IFicheService//Used to create/update a Client
	{
		
        private readonly IRepository<FicheLine, long> _ficheLineRepository;
		//IUnitOfWorkManager _unitOfWorkManager;

        public FicheService(IRepository<Fiche, long> repository,
            IRepository<FicheLine, long> ficheLineRepository)
	: base(repository)
		{
			_ficheLineRepository = ficheLineRepository;
			GetPolicyName = HukukPermissions.Fiches.Default;
			GetListPolicyName = HukukPermissions.Fiches.Default;
			CreatePolicyName = HukukPermissions.Fiches.Create;
			UpdatePolicyName = HukukPermissions.Fiches.Edit;
			DeletePolicyName = HukukPermissions.Fiches.Create;
		}
		public override async Task<PagedResultDto<FicheDto>> GetListAsync(
			PagedAndSortedResultRequestDto input)
		{
			//Get the IQueryable<Book> from the repository
			var queryable = await Repository.GetQueryableAsync();

			//Paging
			queryable = queryable
				.Include(x => x.Client)
				.Include(x => x.FicheLine).ThenInclude(y => y.Item)
				.Include(x => x.ClientTran)
				.Include(x => x.Currency)

				.Skip(input.SkipCount)
				.Take(input.MaxResultCount);

			//Execute the query and get a list
			var queryResult = await AsyncExecuter.ToListAsync(queryable);

			//Convert the query result to a list of BookDto objects
			var Dtos = queryResult.Select(x =>
			{
				var dto = ObjectMapper.Map<Fiche, FicheDto>(x);
				dto.Client = ObjectMapper.Map<Client, ClientDto>(x.Client);
				dto.ClientTran = ObjectMapper.Map<ClientTran, ClientTranDto>(x.ClientTran);
				dto.FicheLine = ObjectMapper.Map<List<FicheLine>, List<FicheLineDto>>(x.FicheLine);
				Console.WriteLine($"Line Count :::::::::FicheId {dto.Id}::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::{dto.FicheLine.Count()}");
				return dto;

			}).ToList();

			//Get the total count with another query
			
			var totalCount = await Repository.GetCountAsync();

			return new PagedResultDto<FicheDto>(
				totalCount,
				Dtos
			);
		}

		[Authorize(Permissions.HukukPermissions.Fiches.Create)]
		public override async Task<FicheDto> CreateAsync(CreateUpdateFicheDto input)
		{
			var ficheLineList = ObjectMapper.Map<List<FicheLineDto>, List<FicheLine>>(input.FicheLine);
			
			Console.WriteLine($"ficheLineList-------------------------------------------{ficheLineList.Count()}");
			input.FicheLine.ForEach(x => { x.Item = null; });
			var myfiche = ObjectMapper.Map<CreateUpdateFicheDto, Fiche>(input);
			var rv = await Repository.InsertAsync(myfiche, true);
			

			Console.WriteLine(rv.Id);

			Console.WriteLine($"debug2---------------------------------------------{ficheLineList.Count()}");

			if (ficheLineList != null)

				foreach (var l in ficheLineList)
				{
					l.FicheId = myfiche.Id;
					l.Item = null;
                                }
			await _ficheLineRepository.InsertManyAsync (ficheLineList);
			
			Console.WriteLine("debug3---------------------------------------------");
			var rvFiche = ObjectMapper.Map<Fiche, FicheDto> (rv);
			rvFiche.FicheLine = new List<FicheLineDto>();

			rv.FicheLine.ForEach(x => { rvFiche.FicheLine.Add(ObjectMapper.Map<FicheLine, FicheLineDto>(x)); });
			return rvFiche;
		}

		public   async Task<PagedResultDto<FicheLineDto>> GetListFichLineAsync (long FicheId, PagedAndSortedResultRequestDto input) {
			//Get the IQueryable<Book> from the repository
			var queryable = await _ficheLineRepository.GetQueryableAsync ();

			//Paging
			queryable = queryable
				.Include (x => x.Item)
				.Include (x => x.Currency)
				.Where(x => x.FicheId == FicheId)
				.Skip (input.SkipCount)
				.Take (input.MaxResultCount);

			//Execute the query and get a list
			var queryResult = await AsyncExecuter.ToListAsync (queryable);

			//Convert the query result to a list of BookDto objects
			var Dtos = queryResult.Select (x => {
				var dto = ObjectMapper.Map<FicheLine, FicheLineDto> (x);
				return dto;

			}).ToList ();

			//Get the total count with another query

			var totalCount = await Repository.GetCountAsync ();

			return new PagedResultDto<FicheLineDto> (
				totalCount,
				Dtos
			);
		}


		[Authorize (Permissions.HukukPermissions.Fiches.Default)]
		public async Task<List<FicheLineDto>> GetListFichLineAsync (long FicheId) {
			//Get the IQueryable<Book> from the repository
			var queryable = await _ficheLineRepository.GetQueryableAsync ();

			//Paging
			queryable = queryable
				.Include(x => x.Item)
				.Include(x => x.Currency)
				.Where(x => x.FicheId == FicheId);
				//.Skip (input.SkipCount)
				//.Take (input.MaxResultCount);

			//Execute the query and get a list
			var queryResult = await AsyncExecuter.ToListAsync (queryable);

			//Convert the query result to a list of BookDto objects
			var Dtos = queryResult.Select (x => {
				var dto = ObjectMapper.Map<FicheLine, FicheLineDto> (x);
				return dto;

			}).ToList ();

			//Get the total count with another query
			return Dtos;
			/*var totalCount = await Repository.GetCountAsync ();
			
			return new PagedResultDto<FicheLineDto> (
				totalCount,
				Dtos
			);
			*/
		}


	}
}
