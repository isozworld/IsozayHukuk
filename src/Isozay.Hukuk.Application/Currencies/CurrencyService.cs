using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Isozay.Hukuk.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;


namespace Isozay.Hukuk.Currencies
{
    [Authorize(HukukPermissions.Clients.Default)]
    public class CurrencyService : ApplicationService, ICurrencyService
    {
        private readonly IRepository<Currency, long> _repositorCurrency;
        public CurrencyService(IRepository<Currency, long> repository)
        {
            _repositorCurrency = repository;
        }
        public  async Task<List<CurrencyDto>> GetListAsync()
        {
            var lst = await _repositorCurrency.GetListAsync();
           return lst.Select(x => ObjectMapper.Map<Currency, CurrencyDto>(x)).ToList();
        }

 
    }
}
