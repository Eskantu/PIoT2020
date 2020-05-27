using PIoT2020.Shared.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.Shared.Modelos
{
    public class ConnectionMQTT
    {
        public bool Connectado { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string ClientID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TopicSubscribe { get; set; }
        public string TopicPublish { get; set; }
        public string Mensaje { get; set; }
        public List<PayloadData> Data { get; set; }
        public List<PayloadData> Messages { get; set; }
        public List<string> Susbscripcions { get; set; }

    }
}
