﻿using System;
using Isozay.Hukuk.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Isozay.Hukuk.Items
{
    public  interface IItemService :
                ICrudAppService< //Defines CRUD methods
            ItemDto, //Used to show books
            long, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            ItemDto> //Used to create/update a book
    {
        Task<IReadOnlyList<ItemDto>> Search(string searchText);
    }
}
