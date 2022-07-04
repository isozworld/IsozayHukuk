using Isozay.Hukuk.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Isozay.Hukuk
{

    [DependsOn(
        typeof(HukukEntityFrameworkCoreTestModule)
        )]
    public class HukukDomainTestModule : AbpModule
    {

    }
}