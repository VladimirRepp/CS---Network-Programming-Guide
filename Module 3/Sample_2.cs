// Задание 3: TCP vs UDP - сравнение надежности
// Цель: Продемонстрировать разницу в надежности между TCP и UDP.
// Код (клиент для тестирования потери пакетов):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Тестирование надежности TCP vs UDP");
        Console.WriteLine("==================================");

        await TestTCPReliability();
        await TestUDPReliability();
    }

    static async Task TestTCPReliability()
    {
        Console.WriteLine("\n--- Тестирование TCP ---");

        using (Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            try
            {
                await tcpClient.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 12345));

                // Отправляем 10 сообщений
                for (int i = 1; i <= 10; i++)
                {
                    string message = $"TCP Message #{i}";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await tcpClient.SendAsync(data, SocketFlags.None);
                    Console.WriteLine($"Отправлено: {message}");

                    // Получаем подтверждение
                    byte[] buffer = new byte[1024];
                    int received = await tcpClient.ReceiveAsync(buffer, SocketFlags.None);
                    string response = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine($"Получено: {response}");

                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка TCP: {ex.Message}");
            }
        }
    }

    static async Task TestUDPReliability()
    {
        Console.WriteLine("\n--- Тестирование UDP ---");

        using (Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Loopback, 12346);
            udpClient.ReceiveTimeout = 2000; // 2 секунды таймаут

            int lostPackets = 0;

            // Отправляем 10 сообщений
            for (int i = 1; i <= 10; i++)
            {
                string message = $"UDP Message #{i}";
                byte[] data = Encoding.UTF8.GetBytes(message);

                try
                {
                    await udpClient.SendToAsync(data, serverEndPoint);
                    Console.WriteLine($"Отправлено: {message}");

                    // Пытаемся получить ответ
                    byte[] buffer = new byte[1024];
                    EndPoint responseEndPoint = new IPEndPoint(IPAddress.Any, 0);

                    try
                    {
                        SocketReceiveFromResult result = await udpClient.ReceiveFromAsync(buffer, responseEndPoint);
                        string response = Encoding.UTF8.GetString(buffer, 0, result.ReceivedBytes);
                        Console.WriteLine($"Получено: {response}");
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        Console.WriteLine($"Пакет #{i} потерян!");
                        lostPackets++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка отправки UDP: {ex.Message}");
                }

                await Task.Delay(100);
            }

            Console.WriteLine($"Потеряно пакетов UDP: {lostPackets}/10");
        }
    }
}