using PIoT2020.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Modelos
{
   public class SimulacionModel
    {
        public Usuario Usuario { get; set; }
        public List<Proyecto> Proyectos { get; set; }
        public List<Dispositivo> Dispositivos { get; set; }
        public List<Sensor> Sensores { get; set; }
        public List<Lectura> Lecturas { get; set; }
    }
}
