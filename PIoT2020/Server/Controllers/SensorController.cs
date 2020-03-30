using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.DAL;

namespace PIoT2020.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SensorController : GenericController<Sensor>
    {
        public SensorController() : base(new GenericRepository<Sensor>())
        {
        }
    }
}