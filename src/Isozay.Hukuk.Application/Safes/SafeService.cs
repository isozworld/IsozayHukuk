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

namespace Isozay.Hukuk.Safes
{
    [Authorize(HukukPermissions.Safes.Default)]
    public class SafeService :
            CrudAppService< //Defines CRUD methods
            Safe,
            SafeDto, //Used to show Clients
            long, //Primary key of the Client entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateSafeDto>, ISafeService//Used to create/update a Client
    {
        public SafeService(IRepository<Safe, long> repository)
    : base(repository)
        {
            GetPolicyName = HukukPermissions.Safes.Default;
            GetListPolicyName = HukukPermissions.Safes.Default;
            CreatePolicyName = HukukPermissions.Safes.Create;
            UpdatePolicyName = HukukPermissions.Safes.Edit;
            DeletePolicyName = HukukPermissions.Safes.Create;
        }

        public override async Task<PagedResultDto<SafeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Include(x => x.Currency)
             .Skip(input.SkipCount)
             .Take(input.MaxResultCount);

             
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x => 
            { 
                var safeDto = ObjectMapper.Map<Safe, SafeDto>(x);
                safeDto.Currency = ObjectMapper.Map<Currency, CurrencyDto>(x.Currency);
                safeDto.CurrencyId = x.CurrencyId;
                return safeDto;
                }).ToList();
            var totalCount = await Repository.GetCountAsync();
 

            return new PagedResultDto<SafeDto>(
                totalCount,
                dtos
);
        }

     


    }
}
