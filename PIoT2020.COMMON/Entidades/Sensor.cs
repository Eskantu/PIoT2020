using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Entidades
{
   public class Sensor:BaseDTO
    {
        public string Name { get; set; }
        public string UnidadDeMedida { get; set; }
        public string IdDispositivo { get; set; }
    }
}
