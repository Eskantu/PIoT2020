using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Entidades
{
   public class Usuario:BaseDTO
    {
        public string UsuarioName { get; set; }
        public string Password { get; set; }
        public string TipoUsuario { get; set; }

    }
}
