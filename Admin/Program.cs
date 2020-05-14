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
                        RealizarSimulacion();
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
        public static void RealizarSimulacion(int usuariosAGenerar = 2000, int diasAPoblar = 30, int dispositivosAGenerar = 10000)
        {
            decimal temperaturaHidalgo = 30;
            decimal lumenes = 25;
            decimal cmsUltrasonico = 25;
            //Usuarios, crear nueva lista de usuarios.
            List<Usuario> usuarios = new List<Usuario>();
            Usuario nuevoUsuario;
            TipoUsuario nuevoTipoUsuario;
            Random r = new Random();
            usuarios = _usuarioManager.ObtenerTodos.ToList();
            if (usuarios.Count < usuariosAGenerar)
            {

                for (int usuarioNumero = 0; usuarioNumero < usuariosAGenerar; usuarioNumero++)
                {
                    nuevoTipoUsuario = _tipoUsuarioManager.Consulta(t => t.Name == "General").SingleOrDefault();

                    nuevoUsuario = _usuarioManager.Crear(new Usuario()
                    {
                        UsuarioName = "Usuario " + usuarioNumero,
                        Password = "P" + r.Next(1, 999) + "A" + r.Next(1, 999) + "S" + r.Next(1, 999) + "S" + r.Next(1, 999),
                        TipoUsuario = nuevoTipoUsuario.Id
                    });
                    usuarios.Add(nuevoUsuario);
                }
            }
            string usuariosCSV = ExportarTablaUsuarios(usuarios);//Exportar tabla usuarios a CSV.
            Imprimir(usuariosCSV);//Imprimir tabla usuarios.
            Console.WriteLine($"{usuarios.Count} usurios creados Creados");

            //Entidades para la simulación.
            Proyecto proyectoNuevo;
            Dispositivo dispositivoNuevo;
            Sensor dht11Nuevo;
            Sensor ultrasonico;
            Sensor fotoresistenciaNuevo;

            //Días.
            for (int i = 0; i < diasAPoblar; i++)
            {
                Console.WriteLine($"Dia: {i}");
                //Dispositivos.
                int dispositivos = 0;
                while (dispositivos <= dispositivosAGenerar)
                {
                    //Usuarios por cada usuario.
                    foreach (var usuarioActual in usuarios)
                    {
                        //Proyectos, proyecto nuevo.

                        proyectoNuevo = _proyectoManager.Crear(new Proyecto { Name = "Proyecto " + usuarioActual.UsuarioName, Descripcion = "Lectura datos " + usuarioActual.Id, IdUsuario=usuarioActual.Id });


                        //Número de dispositivos por usuario.
                        GeneradorCongruencialLineal(6, out decimal numeroDispositivos);
                        for (int j = 0; j < (int)numeroDispositivos; j++)
                        {
                            //Dispositivo nuevo.
                            dispositivoNuevo = _dispositivoManager.Crear(new Dispositivo { Name = "Dispositivo " + (j + 1), Descripcion = "Disp. Usuario " + usuarioActual.UsuarioName + "0" + j, IdProyecto = proyectoNuevo.Id });
                            dispositivos++;

                            //Sensores
                            dht11Nuevo = _sensorManager.Crear(new Sensor { Name = "DHT11 " + (j + 1), UnidadDeMedida = "°C", IdDispositivo = dispositivoNuevo.Id });
                            fotoresistenciaNuevo = _sensorManager.Crear(new Sensor { Name = "Fotoresistencia " + (j + 1), UnidadDeMedida = "LUX", IdDispositivo = dispositivoNuevo.Id });
                            ultrasonico = _sensorManager.Crear(new Sensor { Name = "Ultrasonico " + (j + 1), UnidadDeMedida = "CM", IdDispositivo = dispositivoNuevo.Id });

                            //Tres lecturas al día XD

                            for (int k = 0; k < 3; k++)
                            {
                                GeneradorCongruencialLineal(30, out temperaturaHidalgo);
                                GeneradorCongruencialLineal(25, out lumenes);
                                GeneradorCongruencialLineal(25, out cmsUltrasonico);

                                _lecturaManager.Crear(new Lectura { IdSensor = dht11Nuevo.Id, Value = temperaturaHidalgo });
                                _lecturaManager.Crear(new Lectura { IdSensor = fotoresistenciaNuevo.Id, Value = lumenes });
                                _lecturaManager.Crear(new Lectura { IdSensor = ultrasonico.Id, Value = cmsUltrasonico });
                            }
                        }
                    }
                }
            }
        }
        private static void Imprimir(string usuariosCSV)
        {
            try
            {
                string? ruta = AppDomain.CurrentDomain.BaseDirectory;
                if (string.IsNullOrEmpty(ruta))
                {
                    Console.WriteLine("No se puede guardar el archivo..");
                    return;
                }
                if (Directory.Exists(ruta))
                {
                    StreamWriter archivo = new StreamWriter(ruta+ $@"Simulacion.csv");
                    archivo.Write(usuariosCSV);
                    archivo.Close();
                    Console.WriteLine("Archivo generado correctamente.");
                }
                else
                {
                    Console.WriteLine("La ruta no fue encontrada.");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        private static string ExportarTablaUsuarios(List<Usuario> usuarios)
        {
            string salida = "";
            foreach (var usuario in _usuarioManager.ObtenerTodos)
            {
                salida += usuario.UsuarioName + ", " + usuario.Password + "\r\n";
            }
            return salida;
        }

        #region Generadores de variables aleatorias.
        /// <summary>
        /// Regresa un verdadero o falso, tomando en cuenta una distribución de probabilidad Binomial / Bernoulli.
        /// </summary>
        /// <param name="valor">Valor determinante y limitante.</param>
        /// <returns>Verdadero o Falso.</returns>
        private static bool GeneradorBernoulli(int valor = 50)
        {
            GeneradorCongruencialLineal(valor * 2, out decimal congruencial);
            return 0 <= (int)congruencial && (int)congruencial < valor;//Verdadero o en otro caso Falso.
        }

        /// <summary>
        /// Produce una secuencia de números enteros Xi entre 0 y m - 1.
        /// </summary>
        /// <param name="m">Módulo.</param>
        /// <param name="congruencial">Variable aleatoria siguiente semilla.</param>
        /// <returns>Valor aleatorio entre 0 y m - 1.</returns>
        private static decimal GeneradorCongruencialLineal(decimal m, out decimal congruencial)
        {
            Random r = new Random();
            decimal a = r.Next(1, 101);
            decimal b = r.Next(1, 101);
            decimal x0 = (decimal)r.NextDouble();
            x0 = a * x0;
            x0 += b;
            x0 %= m;
            congruencial = x0;
            return x0 / m;
        }
        #endregion
    }
}
