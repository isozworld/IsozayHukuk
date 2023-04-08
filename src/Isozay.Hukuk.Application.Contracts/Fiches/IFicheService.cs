using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Isozay.Hukuk.Fiches
{
	public interface IFicheService :
		ICrudAppService< //Defines CRUD methods
		FicheDto, //Used to show books
		long, //Primary key of the book entity
		PagedAndSortedResultRequestDto, //Used for paging/sorting
		CreateUpdateFicheDto> //Used to create/update a book
	{
		Task<List<FicheLineDto>> GetListFichLineAsync(long FicheId);
		Task DeleteFicheLine (long FicheLineId);
		Task<Task> CreateFicheLineAsync(FicheLineDto f);
		Task<IReadOnlyList<FicheDto>> Search(string searchText, long id);
        Task UpdateClientTranAsync(long ficheId);
		Task<bool> HasAnyFiches(long id);
		Task<string> getFicheNumber(long? id);
    }
}
