using System;
using System.Collections.Generic;
using System.Text;
using Isozay.Hukuk.Localization;
using Volo.Abp.Application.Services;

namespace Isozay.Hukuk
{

    /* Inherit your application services from this class.
     */
    public abstract class HukukAppService : ApplicationService
    {
        protected HukukAppService()
        {
            LocalizationResource = typeof(HukukResource);
        }
    }
}