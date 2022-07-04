using Volo.Abp.Modularity;

namespace Isozay.Hukuk
{

    [DependsOn(
        typeof(HukukApplicationModule),
        typeof(HukukDomainTestModule)
        )]
    public class HukukApplicationTestModule : AbpModule
    {

    }
}