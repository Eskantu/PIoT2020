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
    public class LecturaController : GenericController<Lectura>
    {
        public LecturaController() : base(new GenericRepository<Lectura>())
        {
        }

        [HttpPost("Insertar")]
        public ActionResult Post(string value, string idSensor)
        {

            try
            {
                if (_genericRepository.Create(new Lectura() { IdSensor=idSensor, Value=value }) is Lectura result)
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
        }
    }