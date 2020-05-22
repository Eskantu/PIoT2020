using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIoT2020.COMMON;

namespace PIoT2020.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MQTTController : ControllerBase
    {
        [HttpGet("Connect")]
        public ActionResult<bool> Get(string broker, int puerto, string idDispositivo, string username=null, string passowrd=null)
        {
            try
            {
                MQTT client = username != null ? new MQTT(broker, puerto, idDispositivo, username, passowrd) : new MQTT(broker, puerto, idDispositivo);
                
                client.DatoRecibido += Client_DatoRecibido;
                client.Desconectado += Client_Desconectado;
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void Client_Desconectado(object sender, string e)
        {
            throw new NotImplementedException();
        }

        private void Client_DatoRecibido(object sender, string e)
        {
            throw new NotImplementedException();
        }

        private void Client_Conectado(object sender, string e)
        {
            throw new NotImplementedException();
        }
    }
}