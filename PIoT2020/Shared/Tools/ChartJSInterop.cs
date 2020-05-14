using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PIoT2020.Shared.Tools
{
    public static class ChartJSInterop
    {
        public async static Task InitializeChart(this IJSRuntime js, string elementId, object[] data, string XKey, string[] yKeys, string[] labels)
        {
            Console.WriteLine($"Registros {data.Length}");
            Console.WriteLine($"{elementId}-{XKey}-{yKeys[0]}-{labels[0]}");
            foreach (var item in data)
            {
                Console.WriteLine($"{item.ToString()}");

            }
            await js.InvokeAsync<object>("LineChart", elementId, data, XKey, yKeys, labels);
        }

    }
}
