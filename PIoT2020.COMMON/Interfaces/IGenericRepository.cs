using PIoT2020.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PIoT2020.COMMON.Interfaces
{
    public interface IGenericRepository<T> where T : BaseDTO
    {
        /// <summary>
        /// Proporciona información sobre el error ocurrido en alguna de las operaciones
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Crea una entidad de tipo 
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns>Si se pudo crear o no la entidad</returns>
        T Create(T entidad);

        /// <summary>
        /// Obtiene todos los registros de la tabla
        /// </summary>
        IEnumerable<T> Read { get; }

        /// <summary>
        /// Actualizar un registro en la tabla en base a la propiedad id
        /// </summary>
        /// <param name="entidad">Entidad ya modificada, el id debe existir en la tabla para modificarse</param>
        /// <returns>Confirmación de la actualización</returns>
        T Update(T entidad);

        /// <summary>
        /// Elimina una entidad en la base de datos de acuerdo al id relacionado
        /// </summary>
        /// <param name="id">Id de la entidad a eliminar</param>
        /// <returns>Confirmación de la eliminación</returns>
        bool Delete(T entidadEliminar);

        /// <summary>
        /// Obtiene una entidad en base a su Id
        /// </summary>
        /// <param name="id">Id de la entidad a obtener</param>
        /// <returns>Entidad completa que le corresponde el Id proporcionado</returns>
        T SearchById(string id);

        /// <summary>
        /// Realiza una consulta personalizada a la tabla
        /// </summary>
        /// <param name="prediacado">Expresión Lambda que define la consulta</param>
        /// <returns>Conjunto de entidades que cumplen con la consulta</returns>
        IEnumerable<T> Query(Expression<Func<T, bool>> predicado);

    }
}
