using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Isozay.Hukuk.Currencies
{
    public  interface ICurrencyService:IApplicationService
    {
        Task<List<CurrencyDto>> GetListAsync();
    }
}
