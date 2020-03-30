using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.BIZ
{
   public class ProyectoManager : GenericManager<Proyecto>, IProyectoManager
    {
        public ProyectoManager(IGenericRepository<Proyecto> genericRepository) : base(genericRepository)
        {
        }
    }
}
