using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PIoT2020.Shared.Tools
{
    public static class FileUtil
    {
        public static ValueTask<object> SaveAs(this IJSRuntime js, string filename, byte[] data)
        {
            return js.InvokeAsync<object>(
                          "saveAsFile",
                          filename,
                          Convert.ToBase64String(data));
        }
    }
}
