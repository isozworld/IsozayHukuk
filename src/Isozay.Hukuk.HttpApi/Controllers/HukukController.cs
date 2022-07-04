using Isozay.Hukuk.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Isozay.Hukuk.Controllers
{

    /* Inherit your controllers from this class.
     */
    public abstract class HukukController : AbpControllerBase
    {
        protected HukukController()
        {
            LocalizationResource = typeof(HukukResource);
        }
    }
}