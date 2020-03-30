using PIoT2020.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Interfaces
{
    public interface IUsuarioManager : IGenericManager<Usuario>
    {
        Usuario Login(string username, string password);
    }
}
