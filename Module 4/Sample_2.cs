// Задание 3: Сравнение Unicast, Broadcast, Multicast
// Цель: Показать разницу между тремя типами рассылки.
// Код (Тестовый клиент):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

class NetworkTestClient
{
    static void Main()
    {
        Console.WriteLine("Тестирование различных типов рассылки");
        Console.WriteLine("1 - Unicast, 2 - Broadcast, 3 - Multicast");
        Console.Write("Выберите тип: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                TestUnicast();
                break;
            case "2":
                TestBroadcast();
                break;
            case "3":
                TestMulticast();
                break;
            default:
                Console.WriteLine("Неверный выбор");
                break;
        }
    }

    static void TestUnicast()
    {
        Console.WriteLine("\n--- Тестирование Unicast ---");

        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            IPEndPoint target = new IPEndPoint(IPAddress.Loopback, 17000);

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                string message = $"Unicast test {i}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                socket.SendTo(data, target);
            }
            sw.Stop();

            Console.WriteLine($"Отправлено 10 unicast сообщений за {sw.ElapsedMilliseconds} мс");
        }
    }

    static void TestBroadcast()
    {
        Console.WriteLine("\n--- Тестирование Broadcast ---");

        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.EnableBroadcast = true;
            IPEndPoint target = new IPEndPoint(IPAddress.Broadcast, 17001);

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                string message = $"Broadcast test {i}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                socket.SendTo(data, target);
            }
            sw.Stop();

            Console.WriteLine($"Отправлено 10 broadcast сообщений за {sw.ElapsedMilliseconds} мс");
        }
    }

    static void TestMulticast()
    {
        Console.WriteLine("\n--- Тестирование Multicast ---");

        using (UdpClient client = new UdpClient())
        {
            IPAddress multicastAddress = IPAddress.Parse("224.1.2.3");
            IPEndPoint target = new IPEndPoint(multicastAddress, 17002);
            client.Ttl = 1;

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                string message = $"Multicast test {i}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data, data.Length, target);
            }
            sw.Stop();

            Console.WriteLine($"Отправлено 10 multicast сообщений за {sw.ElapsedMilliseconds} мс");
        }
    }
}