using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Isozay.Hukuk.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

using Volo.Abp.ObjectMapping;
using Volo.Abp;
using Volo.Abp.Auditing;
using Microsoft.EntityFrameworkCore;
using Isozay.Hukuk.Currencies;
using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Fiches;

namespace Isozay.Hukuk.Safes {
	[Authorize (HukukPermissions.Safes.Default)]
	public class SafeService :
	  CrudAppService< //Defines CRUD methods
	  Safe,
	  SafeDto, //Used to show Clients
	  long, //Primary key of the Client entity
	  PagedAndSortedResultRequestDto, //Used for paging/sorting
	  CreateUpdateSafeDto>, ISafeService //Used to create/update a Client
	{

		public readonly IRepository<SafeTran, long> _safeTranRepository;

        private readonly ClientService _clientService;

        public SafeService (IRepository<Safe, long> repository, IRepository<SafeTran, long> safeTranRepository, ClientService clientService) : base (repository) {
			_clientService = clientService;
			_safeTranRepository = safeTranRepository;
			GetPolicyName = HukukPermissions.Safes.Default;
			GetListPolicyName = HukukPermissions.Safes.Default;
			CreatePolicyName = HukukPermissions.Safes.Create;
			UpdatePolicyName = HukukPermissions.Safes.Edit;
			DeletePolicyName = HukukPermissions.Safes.Create;
		}

		[DisableAuditing]
		public override async Task<PagedResultDto<SafeDto>> GetListAsync (PagedAndSortedResultRequestDto input) {
			var query = await Repository.GetQueryableAsync ();
			query = query.Include (x => x.Currency)
			  .Skip (input.SkipCount)
			  .Take (input.MaxResultCount);

			var queryResult = await AsyncExecuter.ToListAsync (query);

			var dtos = queryResult.Select (x => {
				var safeDto = ObjectMapper.Map<Safe,
				  SafeDto> (x);
				safeDto.Currency = ObjectMapper.Map<Currency, CurrencyDto> (x.Currency);
				safeDto.CurrencyId = x.CurrencyId;
				return safeDto;
			}).ToList ();

			var totalCount = await Repository.GetCountAsync ();

			return new PagedResultDto<SafeDto> (
			  totalCount,
			  dtos
			);
		}

        public async Task<IReadOnlyList<SafeDto>> Search(string searchText)
        {
            var r = await Repository.GetQueryableAsync();
            r = r.Include(x => x.Currency);
            if (!String.IsNullOrEmpty(searchText)) r = r.Where(x => x.Name.Contains(searchText));
            var results = await AsyncExecuter.ToListAsync(r);
            return ObjectMapper.Map<List<Safe>, List<SafeDto>>(results);

        }

        [Authorize (HukukPermissions.Safes.Create)]
		public async Task<SafeTranDto> CreateSafeTran (CreateUpdateSafeTranDto s)
		{
			var safeTran = ObjectMapper.Map<CreateUpdateSafeTranDto, SafeTran>(s);
			await _safeTranRepository.InsertAsync (safeTran);
			var rv = ObjectMapper.Map<SafeTran, SafeTranDto> (safeTran);
            if (rv.ClientId != null) await _clientService.CreateClientTran(rv);
			return rv;
		}

        public async Task<List<SafeTranDto>> GetSafeTransListAsync(long id)
		{
			var queryable = await _safeTranRepository.GetQueryableAsync();
			queryable = queryable
				.Include(x => x.Client)
				.Include(x => x.Safe)
				.Where(x => x.SafeId == id);

			var queryRequest = await AsyncExecuter.ToListAsync(queryable);

			var Dtos = queryRequest.Select(x =>
			{
				var dto = ObjectMapper.Map<SafeTran, SafeTranDto>(x);
				return dto;
			}).ToList();

			return Dtos;

		}

		public async Task<string> getSafeValue(long id)
		{
			decimal safeVal = 0;
			var tranList = await GetSafeTransListAsync(id);
			tranList.ForEach(x =>
			{
				if (x.IO == 'I') safeVal += x.Amount;
				else safeVal -= x.Amount;
			});

			return safeVal.ToString("G29");
		}
	}
}