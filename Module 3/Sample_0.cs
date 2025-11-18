// Задание 1: Простой UDP-сервер (Эхо-сервер)
// Цель: Показать основы работы с UDP-сокетами без установки соединения.
// Код:
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        // Создаем UDP-сокет
        Socket udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Связываем с локальной конечной точкой
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 12346);
        udpServer.Bind(localEndPoint);

        Console.WriteLine("UDP-сервер запущен. Ожидание данных...");

        // Буфер для приема данных
        byte[] buffer = new byte[1024];

        // Конечная точка для хранения информации о клиенте
        EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (true)
            {
                // Получаем данные от любого клиента
                int bytesReceived = udpServer.ReceiveFrom(buffer, ref clientEndPoint);
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

                Console.WriteLine($"Получено от {clientEndPoint}: {receivedData}");

                // Отправляем эхо-ответ тому же клиенту
                string response = $"UDP ECHO: {receivedData}";
                byte[] responseData = Encoding.UTF8.GetBytes(response);
                udpServer.SendTo(responseData, clientEndPoint);

                Console.WriteLine($"Отправлен ответ клиенту {clientEndPoint}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            udpServer.Close();
        }
    }
}