using Isozay.Hukuk.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Isozay.Hukuk.DbMigrator
{

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(HukukEntityFrameworkCoreModule),
        typeof(HukukApplicationContractsModule)
        )]
    public class HukukDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}