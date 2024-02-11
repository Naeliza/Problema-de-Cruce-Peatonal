# Problema de Cruce Peatonal SERVICIO:

Este proyecto contiene un servidor WebSocket en C# que simula dos semaforos e informa al peaton cuando puede cruzar.

# Descripción:

En un cruce peatonal muy transitado, hay semáforos para peatones y vehículos. Diseña un sistema de semáforos que coordine el paso seguro de peatones y vehículos, evitando conflictos y asegurando un flujo eficiente del tráfico.

## Requisitos

- [.NET Core SDK](https://dotnet.microsoft.com/download) instalado en tu sistema.
- Instalar el paquete NuGet `System.Net.WebSockets` para la funcionalidad de WebSocket.

## Instalación de paquetes NuGet

Para instalar el paquete `WebSockets`, puedes utilizar el siguiente comando en la terminal:

```bash
dotnet add package System.Net.WebSockets
```

## Uso

1. Clona este repositorio en tu máquina local:

   ```bash
   git clone <url-del-repositorio>
   ```

2. Abre una terminal y navega hasta el directorio del proyecto.

3. Compila el proyecto utilizando el siguiente comando:

   ```bash
   dotnet build
   ```

4. Ejecuta la aplicación utilizando el siguiente comando:

   ```bash
   dotnet run
   ```

5. El servidor WebSocket estará escuchando en el puerto `9090` del localhost. Asegúrate de tener un cliente WebSocket que pueda conectarse al servidor y enviar mensajes de solicitud de semaforo.

## Repositorio del cliente

[Cliente-WebSocket](https://github.com/Naeliza/Problema-de-Cruce-Peatonal-Cliente]) 

## Contribución

Si encuentras algún problema o tienes alguna sugerencia para mejorar este servidor WebSocket, ¡no dudes en abrir un issue o enviar un pull request!

## Licencia

Este proyecto está bajo la [Licencia MIT](LICENSE).

# ¿Interesado/da en aprender más?

Sitio web: [Naeliza.com](https://naeliza.com/)

Portafolio: [Portafolio Naomi Céspedes](https://naeliza.netlify.app/#home)

Canal de Youtube: [AE Coding](https://www.youtube.com/@AECoding)
