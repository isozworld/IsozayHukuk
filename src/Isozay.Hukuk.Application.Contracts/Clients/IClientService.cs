using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Isozay.Hukuk.Fiches;
using Isozay.Hukuk.Safes;



namespace Isozay.Hukuk.Clients
{
    public  interface IClientService:
                ICrudAppService< //Defines CRUD methods
            ClientDto, //Used to show books
            long, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateClientDto> //Used to create/update a book
           
    {
        public Task<IReadOnlyList<ClientDto>> Search(string searchText);
        public Task<IReadOnlyList<ClientDto>> GetListSearchAsync(string searchText);
        public Task<ClientTranDto> CreateClientTran(FicheDto c);
        public Task<ClientTranDto> CreateClientTran(SafeTranDto c);
        public Task<List<ClientTranDto>> GetClientTranDtoHistory(long id, bool childIncluded);
        public Task<ClientRelationDto> CreateClientRelation(CreateUpdateClientDto c, long ParentId, string desc);
        Task<ClientRelationDto> getParentRelation(long id);
        Task UpdateClientAndRelation(CreateUpdateClientDto c, long childId, long newParentId, string newDescription, bool hadParentBefore);
        Task<bool> isUsed(string s);
        //public Task<string> getClientName(long id);
    }
}
