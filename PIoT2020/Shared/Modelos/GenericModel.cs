using PIoT2020.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.Shared.Modelos
{
   public class GenericModel<T> where T:BaseDTO
    {
        public T EntidadPrincipal { get; set; }
    }
}
