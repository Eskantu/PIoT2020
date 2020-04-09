using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.DAL;
using PIoT2020.Shared.Modelos;
using Microsoft.AspNetCore.Components;

namespace PIoT2020.Server.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class LecturaController : GenericController<Lectura>
    {
        public LecturaController() : base(new GenericRepository<Lectura>())
        {
        }

        [HttpPost("Insertar")]
        public ActionResult Post(decimal value, string idSensor)
        {

            try
            {
                if (_genericRepository.Create(new Lectura() { IdSensor = idSensor, Value = value }) is Lectura result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest($"ERROR: {_genericRepository.Error}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR: {ex.Message}, {_genericRepository.Error}");
            }
        }

        [HttpGet("GetModel")]
        public async Task<List<LecturaModel>> GetModel(string idDispositivo)
        {
            List<LecturaModel> lecturaModels = new List<LecturaModel>();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/");
            var sensores = httpClient.GetJsonAsync<List<Sensor>>("Sensor").Result.Where(p => p.IdDispositivo == idDispositivo).ToList();
            foreach (Sensor sensor in sensores)
            {
                var lecturas = httpClient.GetJsonAsync<List<Lectura>>("Lectura").Result.Where(p => p.IdSensor == sensor.Id).ToList();
                foreach (Lectura lectura in lecturas)
                {
                    lecturaModels.Add(new LecturaModel() { EntidadPrincipal = lectura, Sensor = sensor });
                }
            }
            return lecturaModels;
        }
    }
}