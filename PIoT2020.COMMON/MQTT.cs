using OpenNETCF.MQTT;
using PIoT2020.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PIoT2020.COMMON
{
    public class MQTT : IMQTT
    {
        private readonly MQTTClient clienteMqtt;
        private string _broker;
        private string _device;
        public MQTT(string broker, int puerto, string idDispositivo)
        {
            _broker = broker;
            _device = idDispositivo;
            clienteMqtt = new MQTTClient(broker, puerto);
            clienteMqtt.Connect(idDispositivo);
            while (!clienteMqtt.IsConnected)
            {
                Thread.Sleep(1000);
            }
            CrearEventos();
        }

        public void CrearEventos()
        {
            clienteMqtt.MessageReceived += ClienteMqtt_MessageReceived;
            clienteMqtt.Connected += ClienteMqtt_Connected;
            clienteMqtt.Disconnected += ClienteMqtt_Disconnected;
        }

        public MQTT(string broker, int puerto, string idDispositivo, string username, string passowrd)
        {
            _broker = broker;
            _device = idDispositivo;
            clienteMqtt = new MQTTClient(broker, puerto);
            clienteMqtt.Connect(idDispositivo,username,passowrd);
            while (!clienteMqtt.IsConnected)
            {
                Thread.Sleep(1000);
            }
            clienteMqtt.MessageReceived += ClienteMqtt_MessageReceived;
            clienteMqtt.Connected += ClienteMqtt_Connected;
            clienteMqtt.Disconnected += ClienteMqtt_Disconnected;
        }

        private void ClienteMqtt_Disconnected(object sender, EventArgs e)
        {
            Desconectado(this, "Se ha perido la conexion al broker, verifique su conexion a internet y/o el estado del broker, invoque al metodo Conectar para volver a conectar al broker");
        }

        private void ClienteMqtt_Connected(object sender, EventArgs e)
        {
            Conectado(this, $"Conectado al broker: {_broker}");
        }

        private void ClienteMqtt_MessageReceived(string topic, QoS qos, byte[] payload)
        {
            DatoRecibido(this, System.Text.Encoding.UTF8.GetString(payload));
        }

        public string PublishTopic { get; set; }
        public string SubscriptionTopic { get; set; }

        public event EventHandler<string> DatoRecibido;
        public event EventHandler<string> Desconectado;
        public event EventHandler<string> Conectado;

        public bool Conectar()
        {
            try
            {
                clienteMqtt.Connect(_device);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Publicar(string tema, string value)
        {
            if (clienteMqtt.IsConnected)
            {
                clienteMqtt.Publish(tema, value, QoS.FireAndForget, false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Suscribirse(string tema)
        {
            try
            {
                clienteMqtt.Subscriptions.Add(new Subscription(tema));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
