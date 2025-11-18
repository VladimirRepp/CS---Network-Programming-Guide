// Задание 1: Простой синхронный TCP-сервер (Эхо-сервер)
// Цель: Понять жизненный цикл серверного сокета: Bind -> Listen -> Accept -> Receive/Send -> Close.
// Код:

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        // Создаем сокет для IPv4, работающий по потоковой модели (TCP)
        Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Создаем локальную конечную точку
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 12345); // localhost:12345

        try
        {
            // Связываем сокет с конечной точкой
            listenerSocket.Bind(localEndPoint);
            // Начинаем прослушивание (максимум 10 подключений в очереди)
            listenerSocket.Listen(10);

            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            // Принимаем входящее подключение (этот метод блокирует поток)
            Socket clientSocket = listenerSocket.Accept();
            Console.WriteLine("Клиент подключен!");

            // Буфер для приема данных
            byte[] buffer = new byte[1024];

            // Получаем данные от клиента
            int bytesReceived = clientSocket.Receive(buffer);
            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            Console.WriteLine($"Получено от клиента: {receivedData}");

            // Отправляем данные обратно клиенту (Эхо)
            byte[] echoData = Encoding.UTF8.GetBytes($"ECHO: {receivedData}");
            clientSocket.Send(echoData);

            // Закрываем соединение с клиентом
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            // Закрываем слушающий сокет
            listenerSocket?.Close();
        }
    }
}