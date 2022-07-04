using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Helpers;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic
{
    public partial class MainLayout : IDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime jSRuntime { get; set; }
        [Inject] private SignOutSessionStateManager SignOutManager { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationState { get; set; }

        private bool IsCollapseShown { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += OnLocationChanged;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jSRuntime.InvokeVoidAsync("kaledar.initTheme");
                await jSRuntime.InitializeInactivityTimer(DotNetObjectReference.Create(this));
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void ToggleCollapse()
        {
            IsCollapseShown = !IsCollapseShown;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            IsCollapseShown = false;
            StateHasChanged();
        }

        [JSInvokable]
        public async Task Logout()
        {
            var authState = await AuthenticationState;
            if (authState.User.Identity.IsAuthenticated)
            {
                await SignOutManager.SetSignOutState();
                NavigationManager.NavigateTo("authentication/logout");
            }
        }
    }
}
