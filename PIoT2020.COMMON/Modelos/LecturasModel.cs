using PIoT2020.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Modelos
{
   public class LecturasModel:GenericModel<Lectura>
    {
        public Sensor sensor { get; set; }
    }
}
