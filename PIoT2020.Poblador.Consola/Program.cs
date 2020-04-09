using PIoT2020.BIZ;
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
        static PIoT2020.BIZ.FactoryManager factory = new PIoT2020.BIZ.FactoryManager("Mongo");
        static IActuadorManager _actuadorManager = factory._actuadorManager;
        static IDispositivoManager _dispositivoManager = factory._dispositivoManager;
        static IProyectoManager _proyectoManager = factory._proyectoManager;
        static ISensorManager _sensorManager = factory._sensorManager;
        static ITipoUsuarioManager _tipoUsuarioManager = factory._tipoUsuarioManager;
        static IUsuarioManager _usuarioManager = factory._usuarioManager;
        static ILecturaManager _lecturaManager = factory._lecturaManager;
        static void Main(string[] args)
        {

            bool enPrograma = true;
            do
            {
                Console.WriteLine("PIoT 2020 Menu de administrador");
                Console.WriteLine("1.- Poblacion inicial");
                Console.WriteLine("2.- Generar Lecturas");
                Console.WriteLine("3.- Salir");

                switch (Console.ReadLine())
                {
                    case "1":
                        Inicial();
                        break;
                    case "2":
                        Console.WriteLine("Ingrese el dispositivo al que quiere poblar:");
                        string Id = Console.ReadLine();
                        PoblarDispositivo(Id);
                        break;
                    case "3":
                        enPrograma = false;
                        break;
                    default:
                        Console.WriteLine("Ingrese una opcion valida... presione enter para continuar");
                        break;
                }

            } while (enPrograma);

        }

        private static void PoblarDispositivo(string id)
        {
            Dispositivo dispositivo = _dispositivoManager.BuscarPorId(id);
            List<Sensor> sensores = _sensorManager.ObtenerTodos.Where(p => p.IdDispositivo == dispositivo.Id).ToList();
            Random random = new Random(DateTime.Now.Millisecond);
            foreach (Sensor sensor in sensores)
            {
                Console.WriteLine($"Ingresando lecturas de {sensor.Name}");
                for (int i = 0; i < 20; i++)
                {
                    Lectura lectura = _lecturaManager.Crear(new Lectura() { IdSensor = sensor.Id, Value = random.Next(20, 50) });
                    Console.WriteLine($"{i}.- {lectura.Value}");
                    System.Threading.Thread.Sleep(30000);
                }
            }
            Console.WriteLine("Ingreso de lecturas terminado... presione una tecla para continuar");
        }

        private static void Inicial()
        {
            try
            {

                TipoUsuario tipoUsuarioAdministrador = _tipoUsuarioManager.Crear(new TipoUsuario() { Name = "Administrador" });
                TipoUsuario tipoUsuarioGeneral = _tipoUsuarioManager.Crear(new TipoUsuario() { Name = "General" });
                Usuario usuarioAdministrador = _usuarioManager.Crear(new Usuario() { TipoUsuario = tipoUsuarioAdministrador.Id, UsuarioName = "admin", Password = "qwerty123" });
                Usuario usuarioGeneral = _usuarioManager.Crear(new Usuario() { TipoUsuario = tipoUsuarioGeneral.Id, UsuarioName = "general", Password = "qwerty123" });
                Proyecto proyecto = _proyectoManager.Crear(new Proyecto() { Descripcion = "Proyecto demo para plataforma", IdUsuario = usuarioGeneral.Id, Name = "Proyecto IoT" });
                Dispositivo dispositivo = _dispositivoManager.Crear(new Dispositivo() { Descripcion = "Dispositivo demo", Name = "Proyecto IoT", IdProyecto = proyecto.Id });
                Sensor dht11 = _sensorManager.Crear(new Sensor() { Name = "DHT11", IdDispositivo = dispositivo.Id, UnidadDeMedida = "°C" });
                Sensor ultrasonico = _sensorManager.Crear(new Sensor() { Name = "Ultrsonico", IdDispositivo = dispositivo.Id, UnidadDeMedida = "CM" });
                Sensor fotoresistencia = _sensorManager.Crear(new Sensor() { Name = "Fotoressitencia", IdDispositivo = dispositivo.Id, UnidadDeMedida = "LUX" });
                Console.WriteLine("Poblacion terminada");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}

