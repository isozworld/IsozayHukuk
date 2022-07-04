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
        public FicheService(IRepository<Fiche, long> repository)
    : base(repository)
        {
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
                .Include(x => x.FicheLine)
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
                return dto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<FicheDto>(
                totalCount,
                Dtos
            );
        }


    }
}
