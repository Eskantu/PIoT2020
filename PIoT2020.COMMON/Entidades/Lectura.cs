using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Entidades
{
   public class Lectura:BaseDTO
    {
        public string IdSensor { get; set; }
        public string Value { get; set; }
    }
}
