using OpenNETCF.MQTT;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIoT2020.Shared.Tools
{
    public static class MQTTConnection
    {
        public static event EventHandler<string> DatoRecibido;
        public static event EventHandler<string> Desconectado;
        static MQTTClient clienteMqtt;
        static int t = 0;
        public async static Task<bool> Conectar(string broker, int puerto, string idDispositivo)
        {
            try
            {

                clienteMqtt = clienteMqtt == null ? new MQTTClient(broker, puerto) : clienteMqtt;
                await clienteMqtt.ConnectAsync(idDispositivo);
                while (!clienteMqtt.IsConnected)
                {
                    Thread.Sleep(1000);
                }
                if (t == 0)
                {
                    CrearEventos();
                    t = 1;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async static Task<bool> Desconectar()
        {
            try
            {
                clienteMqtt.Disconnect();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public static void CrearEventos()
        {
            clienteMqtt.MessageReceived += ClienteMqtt_MessageReceived;
            clienteMqtt.Disconnected += ClienteMqtt_Disconnected;
        }

        private static void ClienteMqtt_MessageReceived(string topic, QoS qos, byte[] payload)
        {
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(payload));
            //DatoRecibido(new object(), System.Text.Encoding.UTF8.GetString(payload));
        }
        private static void ClienteMqtt_Disconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Desconectado");
            //Desconectado(new object(), "Se ha perido la conexion al broker, verifique su conexion a internet y/o el estado del broker, invoque al metodo Conectar para volver a conectar al broker");
        }

        public async static Task<bool> Publicar(string tema, string value)
        {
            if (clienteMqtt.IsConnected)
            {
                try
                {
                    await clienteMqtt.PublishAsync(tema, value, QoS.FireAndForget, false);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async static Task<bool> Suscribirse(string tema)
        {
            try
            {
                clienteMqtt.Subscriptions.Add(new Subscription(tema));
                Console.WriteLine($"Se agrego subscripcion: {tema}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
