using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.BIZ
{
    public class DispositivoManager : GenericManager<Dispositivo>, IDispositivoManager
    {
        public DispositivoManager(IGenericRepository<Dispositivo> genericRepository) : base(genericRepository)
        {
        }
    }
}
