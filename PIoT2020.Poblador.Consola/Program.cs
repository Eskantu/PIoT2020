using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIoT2020.Poblador.Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            PIoT2020.BIZ.FactoryManager factory = new PIoT2020.BIZ.FactoryManager("API");

            IActuadorManager _actuadorManager=factory._actuadorManager;
            IDispositivoManager _dispositivoManager=factory._dispositivoManager;
            IProyectoManager _proyectoManager=factory._proyectoManager;
            ISensorManager _sensorManager=factory._sensorManager;
            ITipoUsuarioManager _tipoUsuarioManager=factory._tipoUsuarioManager;
            IUsuarioManager _usuarioManager=factory._usuarioManager;
            TipoUsuario tipoUsuarioAdministrador = _tipoUsuarioManager.Crear(new TipoUsuario() { Name = "Administrador" });
            TipoUsuario tipoUsuarioGeneral = _tipoUsuarioManager.Crear(new TipoUsuario() { Name = "General" });
            Usuario usuarioAdministrador = _usuarioManager.Crear(new Usuario() { TipoUsuario = tipoUsuarioAdministrador.Id, UsuarioName = "admin", Password = "qwerty123" });
            Usuario usuarioGeneral = _usuarioManager.Crear(new Usuario() { TipoUsuario = tipoUsuarioGeneral.Id, UsuarioName = "general", Password = "qwerty123" });
            Actuador LedAmarillo = _actuadorManager.Crear(new Actuador() { Name = "Led Amarillo", MqttCommand = "1" });
            Actuador LedRojo = _actuadorManager.Crear(new Actuador() { Name = "Led Rojo", MqttCommand = "0" });
            Sensor dht11 = _sensorManager.Crear(new Sensor() { Name = "DHT11", UnidadDeMedida = "°C" });
            Sensor distancia = _sensorManager.Crear(new Sensor() { Name = "Ultrasinico", UnidadDeMedida = "cm" });
            Sensor luz = _sensorManager.Crear(new Sensor() { Name = "Foto resistencia", UnidadDeMedida = "Lux" });
            Dispositivo dispositivoPrueba = _dispositivoManager.Crear(new Dispositivo() { Name = "Proyecto practicas", Actuadores = new List<string>() { LedAmarillo.Id, LedRojo.Id }, Sensores = new List<string>() { dht11.Id, luz.Id } });
            Proyecto proyectoIoT = _proyectoManager.Crear(new Proyecto() { Name = "Practias IoT", Dispositivos = new List<string>() { dispositivoPrueba.Id } });
            usuarioGeneral.Proyectos = new List<string>() { proyectoIoT.Id };
            _usuarioManager.Actualizar(usuarioGeneral);

        }

    }
}

