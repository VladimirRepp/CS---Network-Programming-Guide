// Задание 4: Обработка нескольких клиентов (последовательно)
// Цель: Понять, как сервер может обрабатывать более одного клиента, используя цикл.
// Запустите этот сервер и несколько раз запустите клиента из Задания 2. Вы увидите, как они обрабатываются по очереди.
// Код:
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 12345);
        listenerSocket.Bind(localEndPoint);
        listenerSocket.Listen(10);

        Console.WriteLine("Сервер запущен. Ожидание подключений. Нажмите Ctrl+C для выхода.");

        int clientCount = 0;
        while (true) // Бесконечный цикл для принятия многих клиентов
        {
            try
            {
                Socket clientSocket = listenerSocket.Accept();
                clientCount++;
                Console.WriteLine($"Подключен клиент #{clientCount}");

                // Обрабатываем клиента (в том же потоке)
                byte[] buffer = new byte[1024];
                int bytesReceived = clientSocket.Receive(buffer);
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                Console.WriteLine($"[Клиент #{clientCount}] сказал: {receivedData}");

                string response = $"Вы клиент #{clientCount}. Ваше сообщение: '{receivedData}'";
                byte[] responseData = Encoding.UTF8.GetBytes(response);
                clientSocket.Send(responseData);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                Console.WriteLine($"Клиент #{clientCount} отключен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка с клиентом: {ex.Message}");
            }
        }
    }
}
