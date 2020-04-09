using PIoT2020.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.Shared.Modelos
{
    public class DipositivoModel : GenericModel<Dispositivo>
    {
        public List<Sensor> Sensores { get; set; }
        public List<Actuador> Actuadores { get; set; }
        public Proyecto Proyecto { get; set; }

    }
}
