using MongoDB.Bson;
using MongoDB.Driver;
using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PIoT2020.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseDTO
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;
        public GenericRepository()
        {
            _client = new MongoClient(new MongoUrl("mongodb://eskantu:Esklante98@ds016298.mlab.com:16298/piot2020?retryWrites=false"));
            //_client = new MongoClient(new MongoUrl("mongodb://localhost:27017/piot2020?retryWrites=false"));
            _db = _client.GetDatabase("piot2020");
        }
        private IMongoCollection<T> Collection() => _db.GetCollection<T>(typeof(T).Name);
        public string Error { get; private set; }

        public IEnumerable<T> Read
        {
            get
            {
                try
                {
                    IEnumerable<T> datos = Collection().AsQueryable();
                    return datos == null ? throw new Exception("Error en la conexión") : datos;
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    return null;
                }
            }
        }

        public T Create(T entidad)
        {
            try
            {
                entidad.Id = ObjectId.GenerateNewId().ToString();
                entidad.FechaHoraCreacion = DateTime.Now;
                Collection().InsertOne(entidad);
                Error = "";
                return entidad;

            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        public bool Delete(T entidadEliminar)
        {
            try
            {
                int r = (int)Collection().DeleteOne(entidad => entidad.Id==entidadEliminar.Id).DeletedCount;
                Error = r == 1 ? "" : "No se eliminó";
                return r==1 ;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }
        public IEnumerable<T> Query(Expression<Func<T, bool>> predicado) => Read.Where(predicado.Compile());

        public T SearchById(string id) => Read.Where(entidad => entidad.Id.ToString() == id).SingleOrDefault();

        public T Update(T entidadActualizar)
        {
            try
            {
                int r = (int)Collection().ReplaceOne(entidad => entidad.Id == entidadActualizar.Id, entidadActualizar).ModifiedCount;
                Error = r == 1 ? "" : "No se modificó";
                return r == 1 ? entidadActualizar : null;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }
    }
}
