using System;
using System.Collections.Generic;
using System.Text;

namespace PIoT2020.COMMON.Interfaces
{
    interface IMQTT
    {
        string PublishTopic { get; set; }
        string SubscriptionTopic { get; set; }
        bool Publicar(string tema, string value);
        bool Conectar();
        bool Suscribirse(string tema);
        event EventHandler<string> DatoRecibido;
        event EventHandler<string> Desconectado;
        event EventHandler<string> Conectado;

    }
}
