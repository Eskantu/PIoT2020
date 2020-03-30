using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.BIZ
{
   public class FactoryRepository<T> where T:BaseDTO
    {
        string _origen;
        public FactoryRepository(string Origen="Mongo")
        {
            _origen = Origen;
        }

        public string Error { get; private set; }
        public IGenericRepository<T> CrearRepository()
        {
            switch (_origen)
            {
                case "API":
                    Error = "";
                    return new PIoT2020.DAL.API.GenericRepository<T>();
                case "Mongo":
                    Error = "";
                    return new PIoT2020.DAL.GenericRepository<T>();
                default:
                    Error = "El origen de datos no es valido";
                    return null;
            }
        }
    }
}
