// Задание 2: Простой UDP-клиент
// Цель: Показать отправку данных через UDP-сокет без установки соединения.
// Код:
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Конечная точка сервера
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Loopback, 12346);

        try
        {
            Console.Write("Введите сообщение для отправки: ");
            string message = Console.ReadLine();

            // Отправляем данные серверу
            byte[] dataToSend = Encoding.UTF8.GetBytes(message);
            udpClient.SendTo(dataToSend, serverEndPoint);
            Console.WriteLine($"Отправлено серверу: {message}");

            // Буфер для приема ответа
            byte[] buffer = new byte[1024];
            EndPoint serverResponseEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Устанавливаем таймаут на получение (5 секунд)
            udpClient.ReceiveTimeout = 5000;

            try
            {
                int bytesReceived = udpClient.ReceiveFrom(buffer, ref serverResponseEndPoint);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                Console.WriteLine($"Получен ответ от {serverResponseEndPoint}: {response}");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
            {
                Console.WriteLine("Таймаут получения ответа. Возможно, пакет потерян.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            udpClient.Close();
        }
    }
}