using Isozay.Hukuk.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Isozay.Hukuk.Blazor
{

    public abstract class HukukComponentBase : AbpComponentBase
    {
        protected HukukComponentBase()
        {
            LocalizationResource = typeof(HukukResource);
        }
    }
}