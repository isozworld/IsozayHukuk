using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Isozay.Hukuk.Data;
using Volo.Abp.DependencyInjection;

namespace Isozay.Hukuk.EntityFrameworkCore
{

    public class EntityFrameworkCoreHukukDbSchemaMigrator
        : IHukukDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreHukukDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the HukukDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<HukukDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}