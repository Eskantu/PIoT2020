using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIoT2020.BIZ
{
    public class UsuarioManager : GenericManager<Usuario>, IUsuarioManager
    {
        public UsuarioManager(IGenericRepository<Usuario> genericRepository) : base(genericRepository)
        {
        }

        public Usuario Login(string username, string password) => _genericRepository.Query(usuario => usuario.UsuarioName == username && usuario.Password == password).SingleOrDefault();

    }
}
