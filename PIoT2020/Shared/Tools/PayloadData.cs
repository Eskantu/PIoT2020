using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.Shared.Tools
{
   public class PayloadData
    {
        public string Topic { get; set; }
        public string Data { get; set; }
        public string Received { get; set; }

        public PayloadData(string topic, string data)
        {
            this.Topic = topic;
            this.Data = data;
            this.Received = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
