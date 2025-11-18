// Задание 5: Многопоточный TCP-сервер для обработки нескольких клиентов одновременно
// Цель: Показать, как обрабатывать нескольких TCP-клиентов одновременно с помощью потоков.
// Код:
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main()
    {
        Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 12345);
        listenerSocket.Bind(localEndPoint);
        listenerSocket.Listen(10);

        Console.WriteLine("Многопоточный TCP-сервер запущен. Ожидание подключений...");
        int clientCounter = 0;

        while (true)
        {
            Socket clientSocket = listenerSocket.Accept();
            clientCounter++;

            Console.WriteLine($"Принято подключение от клиента #{clientCounter}");

            // Запускаем обработку клиента в отдельном потоке
            Thread clientThread = new Thread(() => HandleClient(clientSocket, clientCounter));
            clientThread.IsBackground = true;
            clientThread.Start();
        }
    }

    static void HandleClient(Socket clientSocket, int clientId)
    {
        try
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesReceived = clientSocket.Receive(buffer);
                if (bytesReceived == 0) // Клиент отключился
                    break;

                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                Console.WriteLine($"[Клиент #{clientId}] сказал: {receivedData}");

                if (receivedData.ToLower() == "exit")
                    break;

                string response = $"[Сервер] Вы клиент #{clientId}. Эхо: {receivedData}";
                byte[] responseData = Encoding.UTF8.GetBytes(response);
                clientSocket.Send(responseData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Клиент #{clientId}] Ошибка: {ex.Message}");
        }
        finally
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            Console.WriteLine($"[Клиент #{clientId}] Отключен");
        }
    }
}