using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.BIZ
{
    public class ActuadorManager : GenericManager<Actuador>, IActuadorManager
    {
        public ActuadorManager(IGenericRepository<Actuador> genericRepository) : base(genericRepository)
        {
        }
    }
}
