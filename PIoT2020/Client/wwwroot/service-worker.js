﻿self.addEventListener('install', async event =>
{
    console.log('instalando el Service worker...');
    self.skipWaiting();
});

self.addEventListener('fetch', event => {
    // Podemos agregar aquí lógica personalizada para controlar 
    // si se pueden utilizar los datos en caché cuando la aplicación 
    // se ejecuta fuera de línea.

    // La siguiente línea hace que las solicitudes vayan directamente 
    // a la red como de costumbre.
    return null;
}); 