using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Admin
{
    class Program
    {
        static IActuadorManager _actuadorManager;
        static IDispositivoManager _dispositivoManager;
        static IProyectoManager _proyectoManager;
        static ISensorManager _sensorManager;
        static ITipoUsuarioManager _tipoUsuarioManager;
        static IUsuarioManager _usuarioManager;
        static ILecturaManager _lecturaManager;

        static void Main(string[] args)
        {
            string? ruta = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Origen de API (Default Local):");
            Console.WriteLine($"1.-{PIoT2020.COMMON.Enumeraciones.ClientAPI.Azure.ToString()}");
            Console.WriteLine($"2.-{PIoT2020.COMMON.Enumeraciones.ClientAPI.IIS.ToString()}");
            Console.WriteLine($"3.-{PIoT2020.COMMON.Enumeraciones.ClientAPI.Local.ToString()}");
            Console.WriteLine($"4.-{PIoT2020.COMMON.Enumeraciones.ClientAPI.VMGoogle.ToString()}");
            Console.WriteLine($"Ingresa el origen de API o cualquier otro numero para default:");
            PIoT2020.COMMON.Enumeraciones.ClientAPI client;
            int r = int.Parse(Console.ReadLine());
            switch (r)
            {
                case 1:
                    client = PIoT2020.COMMON.Enumeraciones.ClientAPI.Azure;
                    break;
                case 2:
                    client = PIoT2020.COMMON.Enumeraciones.ClientAPI.IIS;
                    break;
                case 3:
                    client = PIoT2020.COMMON.Enumeraciones.ClientAPI.Local;
                    break;
                case 4:
                    client = PIoT2020.COMMON.Enumeraciones.ClientAPI.VMGoogle;
                    break;
                default:
                    client = PIoT2020.COMMON.Enumeraciones.ClientAPI.Local;
                    break;
            }
            PIoT2020.BIZ.FactoryManager factory = new PIoT2020.BIZ.FactoryManager(client, "Mongo");
            _actuadorManager = factory._actuadorManager;
            _dispositivoManager = factory._dispositivoManager;
            _proyectoManager = factory._proyectoManager;
            _sensorManager = factory._sensorManager;
            _tipoUsuarioManager = factory._tipoUsuarioManager;
            _usuarioManager = factory._usuarioManager;
            _lecturaManager = factory._lecturaManager;
            decimal temperaturaHidalgo = 30;
            bool enPrograma = true;
            do
            {
                Console.WriteLine("PIoT 2020 Menu de administrador");
                Console.WriteLine("1.- Poblacion inicial");
                Console.WriteLine("2.- Generar Lecturas");
                Console.WriteLine("3.- Realizar Simulación");
                Console.WriteLine("4.- Salir");

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
                       // PIoT2020.BIZ.Tools.RealizarSimulacion();
                        break;
                    case "4":
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
