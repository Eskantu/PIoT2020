using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PIoT2020.BIZ
{
    public class GenericManager<T> : IGenericManager<T> where T : BaseDTO
    {
        internal IGenericRepository<T> _genericRepository;
        public GenericManager(IGenericRepository<T> genericRepository) => _genericRepository = genericRepository;

        public string Error { get { return _genericRepository.Error; } }

        public IEnumerable<T> ObtenerTodos => _genericRepository.Read;

        public T Actualizar(T entidad) => _genericRepository.Update(entidad);

        public T BuscarPorId(string id) => _genericRepository.SearchById(id);

        public IEnumerable<T> Consulta(Expression<Func<T, bool>> predicado) => _genericRepository.Query(predicado);

        public T Crear(T entidad) => _genericRepository.Create(entidad);

        public bool Eliminar(T entidadEliminar) => _genericRepository.Delete(entidadEliminar);
    }
}
