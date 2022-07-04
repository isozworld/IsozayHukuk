using Isozay.Hukuk.Currencies;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Isozay.Hukuk
{
    public class HukukDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Currency, long> _currencyRepository;
        public HukukDataSeederContributor(
            IRepository<Currency, long> currencyRepository
            )
        {
            _currencyRepository = currencyRepository;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await _currencyRepository.InsertAsync(new Currency { Name = "TL" }, autoSave: true);
            await _currencyRepository.InsertAsync(new Currency { Name = "USD" }, autoSave: true);
            await _currencyRepository.InsertAsync(new Currency { Name = "EURO" }, autoSave: true);
        }
    }
}
