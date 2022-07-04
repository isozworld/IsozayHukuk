using Volo.Abp.Settings;

namespace Isozay.Hukuk.Settings
{

    public class HukukSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(HukukSettings.MySetting1));
        }
    }
}