using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.COMMON.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIoT2020.BIZ
{
    public class Tools
    {
        IActuadorManager _actuadorManager;
        IDispositivoManager _dispositivoManager;
        IProyectoManager _proyectoManager;
        ISensorManager _sensorManager;
        ITipoUsuarioManager _tipoUsuarioManager;
        IUsuarioManager _usuarioManager;
        ILecturaManager _lecturaManager;
        List<Usuario> usuarios = new List<Usuario>();
        private Lectura _lectura;
        private Proyecto proyecto;
        private List<Dispositivo> dispositivos = new List<Dispositivo>();

        public List<SimulacionModel> RealizarSimulacion(int usuariosAGenerar, int diasAPoblar, int dispositivosPorUsuarioAGenerar)
        {
            PIoT2020.BIZ.FactoryManager factory = new PIoT2020.BIZ.FactoryManager(PIoT2020.COMMON.Enumeraciones.ClientAPI.Azure, "Mongo");
            _actuadorManager = factory._actuadorManager;
            _dispositivoManager = factory._dispositivoManager;
            _proyectoManager = factory._proyectoManager;
            _sensorManager = factory._sensorManager;
            _tipoUsuarioManager = factory._tipoUsuarioManager;
            _usuarioManager = factory._usuarioManager;
            _lecturaManager = factory._lecturaManager;
            List<SimulacionModel> _simulacionModels = new List<SimulacionModel>();
            List<Sensor> sensors = new List<Sensor>();
            decimal temperaturaHidalgo = 30;
            decimal lumenes = 25;
            decimal cmsUltrasonico = 25;
            //Usuarios, crear nueva lista de usuarios.
            Usuario nuevoUsuario;
            TipoUsuario nuevoTipoUsuario;
            Random r = new Random();

            Parallel.For(0, usuariosAGenerar, usuarioNumero =>
            {
                nuevoTipoUsuario = _tipoUsuarioManager.Consulta(t => t.Name == "General").FirstOrDefault();

                nuevoUsuario = _usuarioManager.Crear(new Usuario()
                {
                    UsuarioName = "Usuario " + Guid.NewGuid().ToString(),
                    Password = "P" + r.Next(1, 999) + "A" + r.Next(1, 999) + "S" + r.Next(1, 999) + "S" + r.Next(1, 999),
                    TipoUsuario = nuevoTipoUsuario.Id
                });
                usuarios.Add(nuevoUsuario);
                Console.WriteLine($"Usuario No.{usuarioNumero}"); ;
                proyecto = _proyectoManager.Crear(new Proyecto { Name = "Proyecto " + nuevoUsuario.UsuarioName, Descripcion = "Proyecto simulacion", IdUsuario = nuevoUsuario.Id });
                Console.WriteLine($"Creando proyecto de usuario"); ;
                for (int i = 0; i < dispositivosPorUsuarioAGenerar; i++)
                {
                    var dispositivo = (_dispositivoManager.Crear(new Dispositivo() { Descripcion = $"Dispositivo simulacion No. {i}", Name = $"Simulacion {i}", IdProyecto = proyecto.Id }));
                    var sensor1 = _sensorManager.Crear(new Sensor() { IdDispositivo = dispositivo.Id, Name = "DHT11", UnidadDeMedida = "°C" });
                    var sensor2 = _sensorManager.Crear(new Sensor() { IdDispositivo = dispositivo.Id, Name = "PhotoCell", UnidadDeMedida = "Lux" });
                    var sensor3 = _sensorManager.Crear(new Sensor() { IdDispositivo = dispositivo.Id, Name = "Ultrasonico", UnidadDeMedida = "CM" });
                    Console.WriteLine($"Usuario No.{i}");
                    sensors = new List<Sensor>() { sensor1, sensor2, sensor3 };
                    dispositivos.Add(dispositivo);

                }
                _simulacionModels.Add(new SimulacionModel() { Usuario = nuevoUsuario, Dispositivos =new List<Dispositivo>(dispositivos) , Proyectos = new List<Proyecto>() { proyecto }, Sensores = new List<Sensor>(sensors), Lecturas=new List<Lectura>() });
                dispositivos.Clear();
                sensors.Clear();
            });
            //Días.
            Parallel.For(0, diasAPoblar, dia =>
            {
                foreach (var item in _simulacionModels)
                {
                    foreach (var sensor in item.Sensores)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            switch (k)
                            {
                                case 0:
                                    GeneradorCongruencialLineal(30, out temperaturaHidalgo);
                                    _lectura = _lecturaManager.Crear(new Lectura { IdSensor = sensor.Id, Value = temperaturaHidalgo, FechaHoraCreacion = DateTime.Now.AddDays(dia).AddHours(5) });
                                    break;
                                case 1:
                                    GeneradorCongruencialLineal(25, out lumenes);
                                    _lectura = _lecturaManager.Crear(new Lectura { IdSensor = sensor.Id, Value = temperaturaHidalgo, FechaHoraCreacion = DateTime.Now.AddDays(dia).AddHours(5) });
                                    break;
                                case 2:
                                    GeneradorCongruencialLineal(25, out cmsUltrasonico);
                                    _lectura = _lecturaManager.Crear(new Lectura { IdSensor = sensor.Id, Value = temperaturaHidalgo, FechaHoraCreacion = DateTime.Now.AddDays(dia).AddHours(5) });
                                    break;
                                default:
                                    break;
                            }
                            item.Lecturas.Add(_lectura);
                        }

                    }
                }
            });
            return _simulacionModels;
        }

        #region Generadores de variables aleatorias.
        /// <summary>
        /// Regresa un verdadero o falso, tomando en cuenta una distribución de probabilidad Binomial / Bernoulli.
        /// </summary>
        /// <param name="valor">Valor determinante y limitante.</param>
        /// <returns>Verdadero o Falso.</returns>
        private bool GeneradorBernoulli(int valor = 50)
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
        private decimal GeneradorCongruencialLineal(decimal m, out decimal congruencial)
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
