using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Isozay.Hukuk.Clients
{
    public  interface IClientService:
                ICrudAppService< //Defines CRUD methods
            ClientDto, //Used to show books
            long, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateClientDto> //Used to create/update a book
           
    {
        public Task<IReadOnlyList<ClientDto>>  Search(string searchText);
    }
}
