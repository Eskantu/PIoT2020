using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.DAL.API;
using System;

namespace PIoT2020.UI.Consola.Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            IGenericRepository<Lectura> repositorySensores = new GenericRepository<Lectura>();
            Lectura sensor = repositorySensores.SearchById("5e76e99b1e215c4bfc19c5ad");
            Lectura lecturaUpdate = repositorySensores.Update(new Lectura() { FechaHoraCreacion = sensor.FechaHoraCreacion, Id = sensor.Id, IdSensor = sensor.IdSensor, Value = "Modificado" });
            if (lecturaUpdate!=null)
            {
                Console.WriteLine(repositorySensores.Delete(lecturaUpdate) ? "Entidad eliminada" : "Errror al eliminar entidad");
            }
        }
    }
}
