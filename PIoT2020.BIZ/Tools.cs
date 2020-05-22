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
    public  class Tools
    {
         IActuadorManager _actuadorManager;
         IDispositivoManager _dispositivoManager;
         IProyectoManager _proyectoManager;
         ISensorManager _sensorManager;
         ITipoUsuarioManager _tipoUsuarioManager;
         IUsuarioManager _usuarioManager;
         ILecturaManager _lecturaManager;
         List<Usuario> usuarios = new List<Usuario>();
        public  List<SimulacionModel> RealizarSimulacion(int usuariosAGenerar, int diasAPoblar, int dispositivosPorUsuarioAGenerar)
        {
            PIoT2020.BIZ.FactoryManager factory = new PIoT2020.BIZ.FactoryManager(PIoT2020.COMMON.Enumeraciones.ClientAPI.Azure, "Mongo");
            _actuadorManager = factory._actuadorManager;
            _dispositivoManager = factory._dispositivoManager;
            _proyectoManager = factory._proyectoManager;
            _sensorManager = factory._sensorManager;
            _tipoUsuarioManager = factory._tipoUsuarioManager;
            _usuarioManager = factory._usuarioManager;
            _lecturaManager = factory._lecturaManager;

            decimal temperaturaHidalgo = 30;
            decimal lumenes = 25;
            decimal cmsUltrasonico = 25;
            //Usuarios, crear nueva lista de usuarios.
            Usuario nuevoUsuario;
            TipoUsuario nuevoTipoUsuario;
            Random r = new Random();

            for (int usuarioNumero = 0; usuarioNumero < usuariosAGenerar; usuarioNumero++)
            {
                nuevoTipoUsuario = _tipoUsuarioManager.Consulta(t => t.Name == "General").SingleOrDefault();

                nuevoUsuario = _usuarioManager.Crear(new Usuario()
                {
                    UsuarioName = "Usuario " + Guid.NewGuid().ToString(),
                    Password = "P" + r.Next(1, 999) + "A" + r.Next(1, 999) + "S" + r.Next(1, 999) + "S" + r.Next(1, 999),
                    TipoUsuario = nuevoTipoUsuario.Id
                });
                usuarios.Add(nuevoUsuario);
            }

            //Entidades para la simulación.
            Proyecto proyectoNuevo;
            Dispositivo dispositivoNuevo;
            Sensor dht11Nuevo;
            Sensor ultrasonico;
            Sensor fotoresistenciaNuevo;

            //Días.
            for (int i = 0; i < diasAPoblar; i++)
            {
                //Dispositivos.
                int dispositivos = 0;
                //Usuarios por cada usuario.
                foreach (var usuarioActual in usuarios)
                {
                    //Proyectos, proyecto nuevo.

                    proyectoNuevo = _proyectoManager.Crear(new Proyecto { Name = "Proyecto " + usuarioActual.UsuarioName, Descripcion = "Lectura datos " + usuarioActual.Id, IdUsuario = usuarioActual.Id });


                    //Número de dispositivos por usuario.
                    for (int j = 0; j < dispositivosPorUsuarioAGenerar; j++)
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
            return ObtenerInformacionSimulada();
        }

        #region Generadores de variables aleatorias.
        /// <summary>
        /// Regresa un verdadero o falso, tomando en cuenta una distribución de probabilidad Binomial / Bernoulli.
        /// </summary>
        /// <param name="valor">Valor determinante y limitante.</param>
        /// <returns>Verdadero o Falso.</returns>
        private  bool GeneradorBernoulli(int valor = 50)
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
        private  decimal GeneradorCongruencialLineal(decimal m, out decimal congruencial)
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

        private  List<SimulacionModel> ObtenerInformacionSimulada()
        {
            List<SimulacionModel> simulacionModels = new List<SimulacionModel>();
            List<Proyecto> proyectos = new List<Proyecto>();
            List<Dispositivo> dispositivos = new List<Dispositivo>();
            List<Sensor> sensores = new List<Sensor>();
            List<Lectura> lecturas = new List<Lectura>();
            foreach (Usuario usuario in _usuarioManager.ObtenerTodos)
            {
                proyectos = _proyectoManager.ObtenerTodos.Where(p => p.IdUsuario == usuario.Id).ToList();
                foreach (Proyecto proyecto in proyectos)
                {
                    dispositivos = _dispositivoManager.ObtenerTodos.Where(p => p.IdProyecto == proyecto.Id).ToList();
                    foreach (Dispositivo dispositivo in dispositivos)
                    {
                        sensores = _sensorManager.ObtenerTodos.Where(p => p.IdDispositivo == dispositivo.Id).ToList();
                        foreach (Sensor sensor in sensores)
                        {
                            lecturas = _lecturaManager.ObtenerTodos.Where(p => p.IdSensor == sensor.Id).ToList();
                        }
                    }
                }
                simulacionModels.Add(new SimulacionModel() { Dispositivos = dispositivos, Lecturas = lecturas, Proyectos = proyectos, Sensores = sensores, Usuario = usuario });
            }
            return simulacionModels;

        }
    }
}
