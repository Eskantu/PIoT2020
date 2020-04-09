using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.DAL;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using PIoT2020.COMMON.Modelos;
using PIoT2020.Shared.Modelos;

namespace PIoT2020.Server.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class DispositivoController : GenericController<Dispositivo>
    {
        public DispositivoController() : base(new GenericRepository<Dispositivo>())
        {
        }

        [HttpGet("CrearDispositivo")]
        public async Task<RedirectResult> CrearDispositivo(string IdProyecto, string name, string descripcion)
        {
            if (_genericRepository.Create(new Dispositivo() { Name = name, IdProyecto=IdProyecto, Descripcion=descripcion }) is Dispositivo result)
            {
                return Redirect($"/ProyectoDetalles/{IdProyecto}");
            }
            else
            {
                return Redirect($"/CrearNuevoDispositivo/{IdProyecto}");
            }
        }

        [HttpGet("GetDispositivoModel")]
        public async Task<List<DipositivoModel>> GetDispositivoModel(string IdProyecto)
        {
            List<DipositivoModel> dipositivoModels = new List<DipositivoModel>();
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/");
            var proyecto = await httpClient.GetJsonAsync<Proyecto>($"Proyecto/{IdProyecto}");
            List<Dispositivo> dispositivos = _genericRepository.Query(p => p.IdProyecto == IdProyecto).ToList();
            foreach (Dispositivo dispositivo in dispositivos)
            {
                var sensores = httpClient.GetJsonAsync<List<Sensor>>("Sensor").Result.Where(p => p.IdDispositivo == dispositivo.Id).ToList();
                var actuadores = httpClient.GetJsonAsync<List<Actuador>>("Actuador").Result.Where(p => p.IdDispositivo == dispositivo.Id).ToList();
                dipositivoModels.Add(new DipositivoModel() { Actuadores = actuadores, EntidadPrincipal = dispositivo, Proyecto = proyecto, Sensores = sensores });
            }
            return dipositivoModels;
        }
    }
}