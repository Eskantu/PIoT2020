using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.BIZ
{
    public class FactoryManager
    {
        public string Origen { get; private set; }
        public FactoryManager(string origen="Mongo")
        {
            Origen = origen;
            //object[] repositorios = new object[] { new FactoryRepository<Actuador>(origen), new FactoryRepository<Dispositivo>(origen), new FactoryRepository<Lectura>(origen), new FactoryRepository<Proyecto>(origen), new FactoryRepository<Sensor>(origen), new FactoryRepository<TipoUsuario>(origen), new FactoryRepository<Usuario>(origen) };
            //foreach (object item in repositorios)
            //{
            //    item.GetType().GetMethod("CrearRepository").Invoke(item, null);
            //}
            switch (origen)
            {
                case "API":
                    _actuadorManager = new ActuadorManager(new  DAL.API.GenericRepository<Actuador>());
                    _dispositivoManager = new DispositivoManager(new DAL.API.GenericRepository<Dispositivo>());
                    _lecturaManager = new LecturaManager(new DAL.API.GenericRepository<Lectura>());
                    _proyectoManager = new ProyectoManager(new DAL.API.GenericRepository<Proyecto>());
                    _sensorManager = new SensorManager(new DAL.API.GenericRepository<Sensor>());
                    _tipoUsuarioManager = new TipoUsuarioManager(new DAL.API.GenericRepository<TipoUsuario>());
                    _usuarioManager = new UsuarioManager(new DAL.API.GenericRepository<Usuario>());
                    break;
                case "Mongo":
                    _actuadorManager = new ActuadorManager(new DAL.GenericRepository<Actuador>());
                    _dispositivoManager = new DispositivoManager(new DAL.GenericRepository<Dispositivo>());
                    _lecturaManager = new LecturaManager(new DAL.GenericRepository<Lectura>());
                    _proyectoManager = new ProyectoManager(new DAL.GenericRepository<Proyecto>());
                    _sensorManager = new SensorManager(new DAL.GenericRepository<Sensor>());
                    _tipoUsuarioManager = new TipoUsuarioManager(new DAL.GenericRepository<TipoUsuario>());
                    _usuarioManager = new UsuarioManager(new DAL.GenericRepository<Usuario>());
                    break;
                default:
                    break;
            }


        }

        public IActuadorManager _actuadorManager;
        public IDispositivoManager _dispositivoManager;
        public ILecturaManager _lecturaManager;
        public IProyectoManager _proyectoManager;
        public ISensorManager _sensorManager;
        public ITipoUsuarioManager _tipoUsuarioManager;
        public IUsuarioManager _usuarioManager;


    }
}
