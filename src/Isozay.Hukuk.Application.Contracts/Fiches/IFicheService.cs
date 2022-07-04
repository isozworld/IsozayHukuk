using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Isozay.Hukuk.Fiches
{
    public  interface IFicheService :
                ICrudAppService< //Defines CRUD methods
            FicheDto, //Used to show books
            long, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateFicheDto> //Used to create/update a book
    {

    }
}
