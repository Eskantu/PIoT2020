using PIoT2020.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Modelos
{
    public class ModeloAPI<T> where T:BaseDTO
    {
        public string Comando { get; set; }
        public string Query { get; set; }
        public T Entidad { get; set; }
    }
}
