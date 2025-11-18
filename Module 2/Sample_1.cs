// Задание 2: Простой синхронный TCP-клиент
// Цель: Понять жизненный цикл клиентского сокета: Connect->Send / Receive->Close.
// Запустите сначала сервер (Задание 1), а затем клиент (Задание 2).
// Код:
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Конечная точка сервера (куда подключаемся)
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Loopback, 12345);

        try
        {
            Console.WriteLine("Подключаемся к серверу...");
            clientSocket.Connect(serverEndPoint);
            Console.WriteLine("Подключено к серверу!");

            // Данные для отправки
            string message = "Привет, сервер!";
            byte[] dataToSend = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(dataToSend);
            Console.WriteLine($"Отправлено серверу: {message}");

            // Буфер для приема ответа
            byte[] buffer = new byte[1024];
            int bytesReceived = clientSocket.Receive(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            Console.WriteLine($"Получено от сервера: {response}");

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}