using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Entidades
{
   public class Actuador:BaseDTO
    {
        public string Name { get; set; }
        public string MqttCommand { get; set; }
    }
}
