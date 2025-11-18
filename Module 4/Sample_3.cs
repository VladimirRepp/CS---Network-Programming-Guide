// Задание 4: Multicast с несколькими группами
// Цель: Показать работу с несколькими multicast-группами.
// Код (Мультигрупповой клиент):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class MultiGroupClient
{
    static void Main()
    {
        Console.WriteLine("Multicast клиент для нескольких групп");
        Console.WriteLine("Подключен к группам: 224.1.1.1 (Новости), 224.2.2.2 (Погода)");

        // Запускаем прием для каждой группы в отдельном потоке
        Thread newsThread = new Thread(() => JoinMulticastGroup("224.1.1.1", 18000, "НОВОСТИ"));
        Thread weatherThread = new Thread(() => JoinMulticastGroup("224.2.2.2", 18000, "ПОГОДА"));

        newsThread.IsBackground = true;
        weatherThread.IsBackground = true;

        newsThread.Start();
        weatherThread.Start();

        Console.WriteLine("Нажмите Enter для выхода...");
        Console.ReadLine();
    }

    static void JoinMulticastGroup(string groupAddress, int port, string groupName)
    {
        UdpClient client = new UdpClient();
        IPAddress multicastAddress = IPAddress.Parse(groupAddress);

        client.JoinMulticastGroup(multicastAddress);
        client.Client.Bind(new IPEndPoint(IPAddress.Any, port));

        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        Console.WriteLine($"[{groupName}] Ожидание сообщений из группы {groupAddress}...");

        while (true)
        {
            try
            {
                byte[] data = client.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"[{groupName}] {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{groupName}] Ошибка: {ex.Message}");
                break;
            }
        }
    }
}