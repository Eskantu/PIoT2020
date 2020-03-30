using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Serialize.Linq.Serializers;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.COMMON.Modelos;

namespace PIoT2020.Server.Controllers
{
    public class GenericController<T> : ControllerBase where T : BaseDTO
    {
        internal IGenericRepository<T> _genericRepository;
        public GenericController(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        // GET: api/Generic
        [HttpGet]
        public ActionResult<List<T>> Get()
        {
            try
            {
                return Ok(_genericRepository.Read);
            }
            catch (Exception ex)
            {

                return BadRequest($"ERROR:{ex.Message}, {_genericRepository.Error}");
            }
        }

        // GET: api/Generic/5
        [HttpGet("{id}")]
        public ActionResult<T> Get(string id)
        {
            try
            {
                if (_genericRepository.SearchById(id) is T objetoEncontrado)
                {
                    return Ok((objetoEncontrado));
                }
                else
                {
                    return NotFound((_genericRepository.Error));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR: {ex.Message}, {_genericRepository.Error}");
            }
        }

        // POST: api/Generic
        [HttpPost]
        public ActionResult Post([FromBody] ModeloAPI<T> value)
        {

            //try
            //{
            //    if (_genericRepository.Create(value) is ModeloAPI<T> result)
            //    {
            //        return Ok(result);
            //    }
            //    else
            //    {
            //        return BadRequest($"ERROR: {_genericRepository.Error}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest($"ERROR: {ex.Message}, {_genericRepository.Error}");
            //}

            try
            {
                switch (value.Comando)
                {
                    case "Query":
                        var serializer = new ExpressionSerializer(new Serialize.Linq.Serializers.JsonSerializer());
                        var query = serializer.DeserializeText(value.Query);
                        return Ok(_genericRepository.Query((Expression<Func<T, bool>>)query).ToList());
                    default:
                        if (_genericRepository.Create(value.Entidad) is T result)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest($"ERROR: {_genericRepository.Error}");
                        }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR: {ex.Message}, {_genericRepository.Error}");
            }
        }

        // PUT: api/Generic/5
        [HttpPut]
        public ActionResult Put([FromBody] ModeloAPI<T> value)
        {
            try
            {
                T entidad = _genericRepository.SearchById(value.Entidad.Id);
                if (entidad == null)
                {
                    return BadRequest($"En la tabla {typeof(T).Name} no se encuentra un registro con ID {value.Entidad.Id}");
                }
                return Ok(_genericRepository.Update(value.Entidad));
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR: {ex.Message}, {_genericRepository.Error}");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                T entidad = _genericRepository.SearchById(id);
                if (entidad == null)
                {
                    return BadRequest($"En la tabla {typeof(T).Name} no se encuentra un registro con ID {id}");
                }
                return Ok(_genericRepository.Delete(entidad));
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR: {ex.Message}, {_genericRepository.Error}");
            }
        }
    }
}
