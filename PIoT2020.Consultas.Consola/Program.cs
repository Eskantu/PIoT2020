using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON;
using PIoT2020.COMMON.Modelos;

namespace PIoT2020.Consultas.Consola
{
    class Program
    {
        static MQTT mqtt;
        static ISensorManager _sensorManager;
        static ILecturaManager _lecturaManager;
        static void Main(string[] args)
        {
            PIoT2020.BIZ.FactoryManager factory = new PIoT2020.BIZ.FactoryManager(PIoT2020.COMMON.Enumeraciones.ClientAPI.Azure,"API");

            //_sensorManager = factory._sensorManager;
            //_lecturaManager = factory._lecturaManager;
            //mqtt = new MQTT("192.168.8.243", 1883, "PIoTConsola");
            //mqtt.Conectado += Mqtt_Conectado;
            //mqtt.Desconectado += Mqtt_Desconectado;
            //mqtt.DatoRecibido += Mqtt_DatoRecibido;
            //mqtt.Suscribirse("practicas/salida");
            bool opcion = true;
            do
            {

                Console.WriteLine("platform for internet of things (PIoT) 2020");
                Console.WriteLine("1.- Consulta");
                Console.WriteLine("2.- Encender/apagar led amarillo");
                Console.WriteLine("3.- Encender/apagar led rojo");
                Console.WriteLine("4.- Mover servo");
                Console.WriteLine("5.- Salir");
                switch (Console.ReadLine())
                {
                    case "1":
                        Consultas();
                        break;
                    case "2":
                        Console.WriteLine(mqtt.Publicar("practicas/entrada", "0") ? "Comando enviado con exito" : "No se pudo inviar el comando, verifique la conexion");
                        break;
                    case "3":
                        Console.WriteLine(mqtt.Publicar("practicas/entrada", "1") ? "Comando enviado con exito" : "No se pudo inviar el comando, verifique la conexion");
                        break;
                    case "4":
                        Console.WriteLine("Ingrese un numero entre 1 y 180, si el numero es menor a 10 ingrese dos ceros antes del numero Ejemplo: 009, 001");
                        if (int.TryParse(Console.ReadLine(), out int numero))
                        {
                            if (numero <= 180 && numero >= 1)
                            {
                                Console.WriteLine(mqtt.Publicar("", "") ? "Comando enviado con exito" : "No se pudo inviar el comando, verifique la conexion");

                            }
                            else
                            {
                                Console.WriteLine("Ingresa un numero entre 1 y 180");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Solo se permiten numero enteros");
                        }
                        break;
                    case "5":
                        opcion = false;
                        break;
                    default:
                        Console.WriteLine("Opicion no valida, ingrese una opcion que este en el menu");
                        break;
                }
            } while (opcion);

        }

        private static void Mqtt_DatoRecibido(object sender, string e)
        {
            //Console.WriteLine(e);
        }

        private static void Mqtt_Desconectado(object sender, string e)
        {
            Console.WriteLine(e);
            mqtt.Conectar();
        }

        private static void Mqtt_Conectado(object sender, string e)
        {
            Console.WriteLine(e);
        }

        private static void Consultas()
        {
            Console.WriteLine("Ingresa una fecha con el siguiente formato (yyyy/mm/dd)");
            List<string> fecha = Console.ReadLine().Split('/').ToList();
            DateTime dateTime = new DateTime(int.Parse(fecha[0]), int.Parse(fecha[1]), int.Parse(fecha[2]));

            Console.WriteLine("Ingresa una fecha mayor a la anterior con el siguiente formato (yyyy/mm/dd)");
            List<string> fecha1 = Console.ReadLine().Split('/').ToList();
            DateTime dateTime1 = new DateTime(int.Parse(fecha1[0]), int.Parse(fecha1[1]), int.Parse(fecha1[2]));
            List<Lectura> lecturas = _lecturaManager.Consulta(l => l.FechaHoraCreacion >= dateTime && l.FechaHoraCreacion <= dateTime1.AddDays(1)).ToList();
            Console.WriteLine("Obteniendo datos...");
            List<LecturasModel> lecturasModels = new List<LecturasModel>();
            foreach (var item in lecturas)
            {
                lecturasModels.Add(new LecturasModel() { EntidadPrincipal = item, sensor = _sensorManager.BuscarPorId(item.IdSensor) });
            }
            foreach (var item in lecturasModels)
            {
                Console.WriteLine($"Sensor: {item.sensor.Name} Lectura tomada: {item.EntidadPrincipal.Value} Fecha de lectura: {item.EntidadPrincipal.FechaHoraCreacion.ToShortDateString()}");
            }
        }
    }
}
