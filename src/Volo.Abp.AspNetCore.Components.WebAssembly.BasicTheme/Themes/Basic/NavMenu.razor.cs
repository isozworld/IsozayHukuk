using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kaledar.HudutGecis.Menu;
using Microsoft.AspNetCore.Components;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic
{
    public partial class NavMenu : IDisposable
    {
        [Inject]
        protected IMenuManager MenuManager { get; set; }

        [Inject]
        protected NavigationManager navManager { get; set; }
        public bool IsHome { get; set; }

        //protected ApplicationMenu Menu { get; set; }
        protected Dictionary<string,ApplicationMenu> Menus { get; set; }

        public void Dispose()
        {
            navManager.LocationChanged -= NavManager_LocationChanged;
        }

        protected override async Task OnInitializedAsync()
        {
            Menus = new Dictionary<string, ApplicationMenu>();
            var menuDetails = await MenuManager.GetAsync(StandardMenus.Main);

            var sections = menuDetails.Items.GroupBy(i => i.CustomData.As<HudutMenuCustomData>()?.Section ?? "Ayarlar")
                               .ToDictionary(group => group.Key, group => group.ToList());

            int i = 0;
            foreach (var section in sections)
            {
                if (menuDetails.Items.Count > 0)
                {
                    i++;
                    var m = new ApplicationMenu($"Kaledar.Menu{i}", section.Key);
                    foreach (var item in section.Value)
                    {
                        m.AddItem(item);
                    }
                    Menus.Add(section.Key, m);
                }
            }
            //Menu = await MenuManager.GetAsync(StandardMenus.Main);

            navManager.LocationChanged += NavManager_LocationChanged;
            
        }

        private void NavManager_LocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            IsHome = (navManager.ToBaseRelativePath(e.Location) == "");
            StateHasChanged();
        }
    }
}
