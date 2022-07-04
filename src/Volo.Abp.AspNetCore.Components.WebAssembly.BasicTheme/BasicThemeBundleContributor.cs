using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    public class BasicThemeBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {
            //context.Add("assets/plugins/global/plugins.bundle.js", true);
            //context.Add("assets/plugins/custom/prismjs/prismjs.bundle.js");
            //context.Add("assets/js/scripts.bundle.js");
        }

        public void AddStyles(BundleContext context)
        {
            //context.Add("_content/Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme/libs/abp/css/theme.css");
            context.Add("assets/plugins/global/plugins.bundle.css");
            context.Add("assets/plugins/custom/prismjs/prismjs.bundle.css");
            context.Add("assets/css/style.bundle.css");
            context.Add("assets/css/themes/layout/header/base/light.css");
            context.Add("assets/css/themes/layout/header/menu/light.css");
            context.Add("assets/css/themes/layout/brand/dark.css");
            context.Add("assets/css/themes/layout/aside/dark.css");
        }
    }
}
