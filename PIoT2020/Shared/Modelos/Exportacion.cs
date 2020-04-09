using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.Shared.Modelos
{
   public class Exportacion
    {
        public int NumeroRegistro { get; set; }
        public DateTime FechaHoraCreacion { get; set; }
        public string Sensor { get; set; }
        public string Valor { get; set; }
    }
}
