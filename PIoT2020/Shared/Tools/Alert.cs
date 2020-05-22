using Microsoft.JSInterop;
using PIoT2020.COMMON.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PIoT2020.Shared.Tools
{
   public static class Alert
    {
        public static async void Load(this IJSRuntime js)
        {
            await js.InvokeAsync<object>("Load");
        }

        public static async void LoadFinished(this IJSRuntime js,string titulo,string texto)
        {
            await js.InvokeAsync<object>("LoadFinished", titulo,texto);
        }
        public static async ValueTask<object> ExportOptions(this IJSRuntime js)
        {
          return  await js.InvokeAsync<object>("ExportOptions");
        }
    }
}
