using System;
using Isozay.Hukuk.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Isozay.Hukuk.Safes
{
	public  interface ISafeService :
		ICrudAppService< //Defines CRUD methods
		SafeDto, //Used to show books
		long, //Primary key of the book entity
		PagedAndSortedResultRequestDto, //Used for paging/sorting
		CreateUpdateSafeDto> //Used to create/update a book
	{
		Task<IReadOnlyList<SafeDto>> Search (string searchText);
		Task<SafeTranDto> MakeClientSafeDeposit(CreateUpdateSafeTranDto s);
		Task<SafeTranDto> MakeClientSafeExpense(CreateUpdateSafeTranDto s);
		Task<SafeTranDto> MakeSafeTransfer(CreateUpdateSafeTranDto s);
        Task<List<SafeTranDto>> GetSafeTransListAsync(long id);
		Task<string> getSafeValue(long id);
        Task<string> getSafeName(long? id);
        
    }
}
