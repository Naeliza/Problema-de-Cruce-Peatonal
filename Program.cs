using System.Net;
using System.Net.WebSockets;
using System.Text;

class Program
{
    static SemaphoreSlim semaphorePedestrian = new(1);
    static SemaphoreSlim semaphoreVehicle = new(1);
    static bool isPedestrianGreen = false;
    static bool isVehicleGreen = true; // Cambiado a verde
    static WebSocket serverWebSocket = null;
    static System.Timers.Timer timer;

    static async Task Main(string[] args)
    {
        // Configuración del servidor HTTP
        using HttpListener listener = new();
        listener.Prefixes.Add("http://localhost:9090/");
        listener.Start();
        Console.WriteLine("Escuchando en http://localhost:9090/");

        // Tarea para cambiar el semáforo de forma periódica
        Task changeSemaphoreTask = ChangeSemaphore();

        // Inicialización y configuración del temporizador para enviar actualizaciones periódicas a los clientes
        timer = new System.Timers.Timer();
        timer.Interval = 5000; // Intervalo de actualización en milisegundos (5 segundos)
        timer.Elapsed += async (sender, e) => await SendSemaphoreStatus(); // Manejador de evento
        timer.AutoReset = true; // El temporizador se reinicia automáticamente después de cada intervalo
        timer.Enabled = true; // El temporizador está activado y listo para comenzar

        // Bucle principal para manejar las solicitudes de clientes
        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                await ProcessRequest(context); // Procesa la solicitud del cliente WebSocket
            }
            else
            {
                context.Response.StatusCode = 400; // Código de respuesta de error
                context.Response.Close();
            }
        }
    }

    // Método para procesar las solicitudes de los clientes WebSocket
    static async Task ProcessRequest(HttpListenerContext context)
    {
        HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
        WebSocket webSocket = webSocketContext.WebSocket;

        Console.WriteLine("Cliente conectado.");
        serverWebSocket = webSocket; // Guarda la referencia al WebSocket del cliente

        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                // Recibe mensajes del cliente
                byte[] buffer = new byte[1024];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string request = Encoding.UTF8.GetString(buffer, 0, result.Count).Trim().ToLower();

                // Si el cliente solicita cambiar el semáforo de peatones a verde
                if (request == "s")
                {
                    await semaphorePedestrian.WaitAsync();
                    isPedestrianGreen = true;
                    semaphorePedestrian.Release();
                    await semaphoreVehicle.WaitAsync();
                    isVehicleGreen = false; // Cambiar el semáforo de vehículos a rojo
                    semaphoreVehicle.Release();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Método para enviar el estado del semáforo a los clientes WebSocket
    static async Task SendSemaphoreStatus()
    {
        if (serverWebSocket != null && serverWebSocket.State == WebSocketState.Open)
        {
            // Determina el estado del semáforo para peatones y vehículos
            string pedestrianStatus = isPedestrianGreen ? "VERDE" : "ROJO";
            string vehicleStatus = isVehicleGreen ? "VERDE" : "ROJO";

            string message = $"Semaforo para peatones: {pedestrianStatus}\n" +
                             $"Semaforo para vehículos: {vehicleStatus}";

            Console.WriteLine(message);

            // Convierte el mensaje a bytes y lo envía al cliente WebSocket
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await serverWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        else
        {
            Console.WriteLine("No hay clientes conectados o la conexión está cerrada.");
        }
    }

    // Método para cambiar el estado del semáforo de forma periódica
    static async Task ChangeSemaphore()
    {
        while (true)
        {
            await semaphorePedestrian.WaitAsync();
            isPedestrianGreen = false; // Cambia el estado del semáforo de peatones a rojo
            semaphorePedestrian.Release();

            // Espera 20 segundos antes de cambiar el semáforo de peatones a verde
            await Task.Delay(20000);

            await semaphorePedestrian.WaitAsync();
            isPedestrianGreen = true; // Cambia el estado del semáforo de peatones a verde
            semaphorePedestrian.Release();

            await semaphoreVehicle.WaitAsync();
            isVehicleGreen = true; // Cambia el estado del semáforo de vehículos a verde
            semaphoreVehicle.Release();
        }
    }
}
