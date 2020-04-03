using Newtonsoft.Json;
using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.COMMON.Modelos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serialize.Linq.Serializers;

namespace PIoT2020.DAL.API
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseDTO
    {
        HttpClient httpClient;
        string _apiEntidad;
        public GenericRepository()
        {
            httpClient = new HttpClient();
            _apiEntidad = typeof(T).Name;
            //httpClient.BaseAddress = new Uri("https://marioescalante.azurewebsites.net/");
            ///httpClient.BaseAddress = new Uri("https://localhost:44372/");
            httpClient.BaseAddress = new Uri("http://localhost/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.MaxResponseContentBufferSize = 100000000;
        }
        public string Error { get; private set; }

        public IEnumerable<T> Read => ObtenerDatos().Result;


        public T Create(T entidad) => CrearDato(entidad).Result;

        public bool Delete(T entidad) => Eliminar(entidad).Result;

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicado) => Consulta(predicado).Result;


        public T SearchById(string id)
        {
            return BuscarPorId(id).Result;
        }

        public T Update(T entidad)
        {
            return Actualizar(entidad).Result;
        }


        #region Implementación de métodos

        #endregion

        private async Task<IEnumerable<T>> ObtenerDatos()
        {
            HttpResponseMessage message = await httpClient.GetAsync(_apiEntidad).ConfigureAwait(false);
            var content = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (message.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(content);

            }
            else
            {
                Error = content;
                return null;
            }
        }

        private async Task<T> CrearDato(T entidad)
        {
            var r = JsonConvert.SerializeObject(new ModeloAPI<T>() { Entidad = entidad, Query = "", Comando = "Insert" });
            var content = new StringContent(r, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await httpClient.PostAsync(_apiEntidad, content).ConfigureAwait(false);
            var result = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (message.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                Error = result;
                return null;
            }


        }

        private async Task<IEnumerable<T>> Consulta(Expression<Func<T, bool>> predicado)
        {
            var serializer = new ExpressionSerializer(new Serialize.Linq.Serializers.JsonSerializer());
            var json = serializer.SerializeText(predicado);
            ModeloAPI<T> content = new ModeloAPI<T>() { Comando = "Query", Query = json };
            HttpResponseMessage message = await httpClient.PostAsync(_apiEntidad, new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var result = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (message.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(result);
            }
            else
            {
                Error = result;
                return null;
            }
        }

        private async Task<T> Actualizar(T entidad)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new ModeloAPI<T>() { Entidad = entidad, Query = "", Comando = "Actualizar" }), Encoding.UTF8, "application/json");
            HttpResponseMessage message = await httpClient.PutAsync(_apiEntidad, content).ConfigureAwait(false);
            var result = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (message.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                Error = result;
                return null;
            }
        }

        private async Task<bool> Eliminar(T entidad)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new ModeloAPI<T>() { Entidad = entidad, Query = "", Comando = "Eliminar" }), Encoding.UTF8, "application/json");
            HttpResponseMessage message = await httpClient.DeleteAsync(_apiEntidad + $"/{entidad.Id}").ConfigureAwait(false);
            var result = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (message.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<bool>(result);
            }
            else
            {
                Error = result;
                return false;
            }
        }

        private async Task<T> BuscarPorId(string id)
        {
            HttpResponseMessage message = await httpClient.GetAsync(_apiEntidad + $"/{id}").ConfigureAwait(false);
            var content = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (message.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(content);

            }
            else
            {
                Error = content;
                return null;
            }
        }
    }
}
