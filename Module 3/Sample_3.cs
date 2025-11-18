// Задание 4: UDP-клиент с "подключением"
// Цель: Показать использование Connect() с UDP-сокетом для упрощения обмена данными.
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

        try
        {
            // "Подключаемся" к серверу (фиксируем удаленную конечную точку)
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Loopback, 12346);
            udpClient.Connect(serverEndPoint);

            Console.WriteLine("UDP 'подключение' установлено. Введите сообщения:");

            while (true)
            {
                Console.Write("> ");
                string message = Console.ReadLine();

                if (string.IsNullOrEmpty(message))
                    break;

                // Теперь можно использовать Send/Receive вместо SendTo/ReceiveFrom
                byte[] dataToSend = Encoding.UTF8.GetBytes(message);
                int bytesSent = udpClient.Send(dataToSend);
                Console.WriteLine($"Отправлено {bytesSent} байт");

                // Получаем ответ
                byte[] buffer = new byte[1024];
                try
                {
                    udpClient.ReceiveTimeout = 3000;
                    int bytesReceived = udpClient.Receive(buffer);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    Console.WriteLine($"Ответ: {response}");
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    Console.WriteLine("Ответ не получен (таймаут)");
                }
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