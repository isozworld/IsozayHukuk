using System;
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

    }
}
