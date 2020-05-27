using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.Shared.Tools
{
    public static class MqttJsInterop
    {

        public static async void CreateClient(this IJSRuntime js, string webSocket_URL, int port, string clientId,bool useSW)
        {
            await js.InvokeVoidAsync(
                "Connect",
                webSocket_URL, port, clientId, useSW);
        }

        public static async void Subscribe(this IJSRuntime js, string topic, int qos)
        {
            await js.InvokeVoidAsync(
                "Subscribe",
                topic, qos);
        }
        public static async void Publish(this IJSRuntime js, string topic, string payload)
        {
            await js.InvokeVoidAsync(
                "Publish",
                topic, payload);
        }
    }
}
