// Задание 3: Асинхронное принятие подключения на сервере
// Цель: Познакомиться с асинхронной моделью работы сокетов.
// Код (модификация сервера из Задания 1):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 12345);

        try
        {
            listenerSocket.Bind(localEndPoint);
            listenerSocket.Listen(10);
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            // Асинхронное ожидание подключения
            Socket clientSocket = await listenerSocket.AcceptAsync();
            Console.WriteLine("Клиент подключен!");

            // Работа с клиентом (пока синхронно для простоты)
            byte[] buffer = new byte[1024];
            int bytesReceived = clientSocket.Receive(buffer);
            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            Console.WriteLine($"Получено: {receivedData}");

            byte[] echoData = Encoding.UTF8.GetBytes($"ASYNC ECHO: {receivedData}");
            clientSocket.Send(echoData);

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            listenerSocket?.Close();
        }
    }
}