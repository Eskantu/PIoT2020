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
using Microsoft.AspNetCore.Components;
using PIoT2020.Shared.Modelos;
using PIoT2020.Shared.Tools;
using PIoT2020.COMMON.Modelos;
using System.Linq.Expressions;

namespace PIoT2020.Server.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class ProyectoController : GenericController<Proyecto>
    {
        public ProyectoController() : base(new GenericRepository<Proyecto>())
        {
        }

        [HttpGet("CrearNuevoProyecto")]
        public async Task<RedirectResult> CrearNuevoProyecto(string Nombre, string descripcion, string idUsuario)
        {
            if (_genericRepository.Create(new Proyecto() { Name=Nombre, IdUsuario=idUsuario, Descripcion=descripcion}) is Proyecto result)
            {
                return Redirect("/DashboardGeneral");
            }
            else
            {
                return Redirect("/CrearProyecto");
            }
        }
        [HttpGet("GetDetallesProyecto")]
        public async Task<ProyectoModel> DetallesProyecto(string id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/");
            var proyecto = await httpClient.GetJsonAsync<Proyecto>($"Proyecto/{id}");
            List<Dispositivo> dispositivosProyecto = new List<Dispositivo>();
            dispositivosProyecto = httpClient.GetJsonAsync<List<Dispositivo>>("Dispositivo").Result.Where(p=>p.IdProyecto==id).ToList();
            return new ProyectoModel() { Dispositivos = dispositivosProyecto, EntidadPrincipal = proyecto };

        }

        [HttpGet("GetModelo")]
        public async Task<List<ProyectoModel>> GetModelo()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/");
            var proyectos = await httpClient.GetJsonAsync<List<Proyecto>>($"Proyecto");
            List<ProyectoModel> proyectoModels = new List<ProyectoModel>();
            List<Actuador> actuadores =new List<Actuador>();
            List<Sensor> sensores =new List<Sensor>();
            foreach (var item in proyectos)
            {
                var dispositivos = httpClient.GetJsonAsync<List<Dispositivo>>("Dispositivo").Result.Where(p => p.IdProyecto == item.Id).ToList();
                proyectoModels.Add(new ProyectoModel() { Dispositivos = dispositivos, EntidadPrincipal = item});
            }
            return proyectoModels;

        }
    }
}