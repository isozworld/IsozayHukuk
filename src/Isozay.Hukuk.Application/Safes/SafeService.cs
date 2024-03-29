﻿using System;
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
        public readonly IRepository<Fiche, long> _ficheRepository;

        private readonly ClientService _clientService;

        public SafeService (IRepository<Safe, long> repository,
			IRepository<SafeTran, long> safeTranRepository,
            IRepository<Fiche, long> ficheRepository,
            ClientService clientService
			) : base (repository) {
			_ficheRepository = ficheRepository;
			_clientService = clientService;
			_safeTranRepository = safeTranRepository;
			GetPolicyName = HukukPermissions.Safes.Default;
			GetListPolicyName = HukukPermissions.Safes.Default;
			CreatePolicyName = HukukPermissions.Safes.Create;
			UpdatePolicyName = HukukPermissions.Safes.Edit;
			DeletePolicyName = HukukPermissions.Safes.Create;
		}

		public async Task<List<SafeDto>> GetSafeListAsync(SafeType t)
		{
			var safes = (await Repository.GetQueryableAsync()).Where(x => x.SafeType == t).ToList();
			var rv = new List<SafeDto>();
			safes.ForEach(x => { rv.Add(ObjectMapper.Map<Safe, SafeDto>(x)); });
			return rv;
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

        public async Task<SafeTranDto> MakeSafeTransfer(CreateUpdateSafeTranDto s)
		{
            var safeTran = ObjectMapper.Map<CreateUpdateSafeTranDto, SafeTran>(s);
            await _safeTranRepository.InsertAsync(safeTran);
            var rv = ObjectMapper.Map<SafeTran, SafeTranDto>(safeTran);
            return rv;
        }

		public async Task<SafeTranDto> MakeClientSafeDeposit (CreateUpdateSafeTranDto s)
		{
			var safeTran = ObjectMapper.Map<CreateUpdateSafeTranDto, SafeTran>(s);
			await _safeTranRepository.InsertAsync (safeTran);
			var rv = ObjectMapper.Map<SafeTran, SafeTranDto> (safeTran);
			rv.Fiche = ObjectMapper.Map<Fiche,FicheDto>(await _ficheRepository.GetAsync(rv.FicheId ?? default));
            await _clientService.CreateClientTran(rv, 'O');
			return rv;
		}

        public async Task<SafeTranDto> MakeClientSafeExpense(CreateUpdateSafeTranDto s)
		{
            var safeTran = ObjectMapper.Map<CreateUpdateSafeTranDto, SafeTran>(s);
            await _safeTranRepository.InsertAsync(safeTran);
            var rv = ObjectMapper.Map<SafeTran, SafeTranDto>(safeTran);
            rv.Fiche = ObjectMapper.Map<Fiche, FicheDto>(await _ficheRepository.GetAsync(rv.FicheId ?? default));
            await _clientService.CreateClientTran(rv, 'I');
            return rv;
        }

        public async Task<List<SafeTranDto>> GetSafeTransListAsync(long id)
		{
			var queryable = await _safeTranRepository.GetQueryableAsync();
			queryable = queryable
				.Include(x => x.Client)
				.Include(x => x.Safe)
				.Include(x => x.Fiche)
				.Where(x => x.SafeId == id);

			var queryRequest = await AsyncExecuter.ToListAsync(queryable);

			var Dtos = queryRequest.Select(x =>
			{
				var dto = ObjectMapper.Map<SafeTran, SafeTranDto>(x);
				return dto;
			}).ToList();

            SafeTranDto previous = null;

            foreach (SafeTranDto current in Dtos)
            {

                if (current.IO == 'O')
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

		public async Task<string> getSafeName(long? id)
		{
			if (id == null) return "";
			else return (await Repository.GetAsync(id ?? default)).Name;
		}

		
	}
}