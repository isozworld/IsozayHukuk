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
		Task DeleteFicheLine (List<long> f);
        Task DeleteFicheInstallments(List<long> f);
        Task CreateFicheLineAsync(FicheLineDto f);
		Task CreateFicheInstallmentAsync(FicheInstallmentDto f);
        Task CreateFicheInstallmentManyAsync(List<FicheInstallmentDto> f);
        Task CreateFicheLineManyAsync(List<FicheLineDto> f);
        Task<IReadOnlyList<FicheDto>> Search(string searchText, long id);
        Task UpdateClientTranAsync(long ficheId);
		Task<bool> HasAnyFiches(long id);
		Task<string> getFicheNumber(long? id);
		Task UpdateFicheLines(List<FicheLineDto> f);
        Task UpdateFicheInstallments(List<FicheInstallmentDto> f);
        Task<List<FicheInstallmentDto>> GetFicheInstallmentsAsync(long id);
		Task<List<FicheInstallmentDto>> GetSortedFicheInstallmentsAsync();
        Task<List<FicheInstallmentDto>> FillInstallments(int months, DateTime initial);
    }
}
