using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Entidades
{
    public class Proyecto : BaseDTO
    {
        public string Name { get; set; }
        public string IdUsuario { get; set; }
        public string Descripcion { get; set; }
    }
}
