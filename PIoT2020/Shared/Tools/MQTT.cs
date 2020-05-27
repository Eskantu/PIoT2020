using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.Shared.Tools
{
    public static class OnMessageReceived
    {
        public static Action<string, string> Action { get; set; }
        public static void Handler(string key, string data)
        {
            Action?.Invoke(key, data);
        }
    }
    public static class OnConnectionChanged
    {
        public static Action<string> Action { get; set; }
        public static void Handler(string value)
        {
            Action?.Invoke(value);
        }
    }

}
