using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIoT2020.COMMON.Modelos;
using Microsoft.JSInterop;
namespace PIoT2020.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SimulacionController : ControllerBase
    {
        [HttpGet("simular")]
        public async Task<List<SimulacionModel>> Get(int usuarios,int dias, int dispositivosPorUsuario)
        {
            BIZ.Tools tools = new BIZ.Tools();
            try
            {
                var r = tools.RealizarSimulacion(usuarios,dias,dispositivosPorUsuario);
                return r;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}