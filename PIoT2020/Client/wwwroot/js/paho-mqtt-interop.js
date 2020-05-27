const mqttAssemblyName = "PIoT2020.Shared";
const mqttNamespace = "PIoT2020.Shared.Tools";
const mqttMethodName = "Handler";

const mqttOnConnectionChangedTypeName = "OnConnectionChanged";
const mqttOnMessageReceivedTypeName = "OnMessageReceived";

var client;
function Connect(webSocket_URL, port, ClientId, useWS) {
    const options =
    {
        connectTimeout: 4000,
        clientId: ClientId,
        keeplive: 60,
        clean: true
    }
    if (useWS) {
        client = mqtt.connect('ws://'+webSocket_URL + ":" + port+"/mqtt", options);
    }
    else {
        client = mqtt.connect(webSocket_URL + ":" + port, options);
    }

    client.on('close', () => {
        console.log('Mqtt Desconectado')
        var methodOnConnectionChanged = Blazor.platform.findMethod(mqttAssemblyName, mqttNamespace, mqttOnConnectionChangedTypeName, mqttMethodName);
        Blazor.platform.callMethod(methodOnConnectionChanged, null, [Blazor.platform.toDotNetString("Desconnected")]);
    })
    client.on('connect', () => {
        console.log('Mqtt conectado')
        var methodOnConnectionChanged = Blazor.platform.findMethod(mqttAssemblyName, mqttNamespace, mqttOnConnectionChangedTypeName, mqttMethodName);
        Blazor.platform.callMethod(methodOnConnectionChanged, null, [Blazor.platform.toDotNetString("Connected")]);

    })
    client.on('message', (topic, message) => {
        console.log('mensaje recibido bajo topico:', topic, '->', message.toString());
        var methodOnMessagedReceived = Blazor.platform.findMethod(mqttAssemblyName, mqttNamespace, mqttOnMessageReceivedTypeName, mqttMethodName);
        Blazor.platform.callMethod(methodOnMessagedReceived, null, [Blazor.platform.toDotNetString(topic), Blazor.platform.toDotNetString(message.toString())]);
    })
}
function Subscribe(topic, qos) {
    client.subscribe(topic, { qos: qos }, (error) => {
        if (!error) {
            console.log('subscripcion exitosa!')
        }
        else {
            console.log('Susbscripcion failla')

        }
    });
}

function Publish(topic, message) {
    client.publish(topic, message, (error) => {
        if (!error) {
            console.log('Mensaje enviado');
        }
        else {
            console.log('Error');
        }
    });
}