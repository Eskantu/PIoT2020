using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Entidades
{
   public class Dispositivo:BaseDTO
    {
        public string Name { get; set; }
        public List<string> Sensores { get; set; }
        public List<string> Actuadores { get; set; }
    }
}
