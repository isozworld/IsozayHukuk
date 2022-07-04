using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Isozay.Hukuk
{

    [Dependency(ReplaceServices = true)]
    public class HukukBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Hukuk";
    }
}