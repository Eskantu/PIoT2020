using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PIoT2020.Shared.Modelos
{
    public class SimulacionFormModel
    {
        public int NumeroUsuarios { get; set; } = 1;
        public int Dias { get; set; } = 1;
        public int DispositvosPorUsuario { get; set; } = 1;
    }
}
