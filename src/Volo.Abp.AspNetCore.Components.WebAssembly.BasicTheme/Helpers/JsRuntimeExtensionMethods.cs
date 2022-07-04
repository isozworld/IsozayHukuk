using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Helpers
{
    public static class JsRuntimeExtensionMethods
    {
        public static async ValueTask InitializeInactivityTimer<T>(this IJSRuntime js,
            DotNetObjectReference<T> dotNetObjectReference) where T : class
        {
            await js.InvokeVoidAsync("kaledar.initializeInactivityTimer", dotNetObjectReference);
        }
    }
}
