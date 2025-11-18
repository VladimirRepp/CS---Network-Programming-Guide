// Задание 2: Simple Multicast - присоединение к группе
// Цель: Показать основы multicast-коммуникации.
// Код (Multicast Sender):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class MulticastSender
{
    static void Main()
    {
        UdpClient sender = new UdpClient();

        // Multicast группа: 224.5.6.7, порт: 16000
        IPAddress multicastAddress = IPAddress.Parse("224.5.6.7");
        int port = 16000;

        // Устанавливаем TTL = 10 для маршрутизации через несколько сетей
        sender.Ttl = 10;

        Console.WriteLine("Multicast отправитель запущен. Отправка сообщений в группу 224.5.6.7...");

        int messageCount = 0;
        while (true)
        {
            string message = $"Multicast сообщение #{++messageCount} от {DateTime.Now:T}";
            byte[] data = Encoding.UTF8.GetBytes(message);

            sender.Send(data, data.Length, new IPEndPoint(multicastAddress, port));
            Console.WriteLine($"Отправлено в группу: {message}");

            Thread.Sleep(2000); // Отправляем каждые 2 секунды
        }
    }
}

// Код (Multicast Receiver):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class MulticastReceiver
{
    static void Main()
    {
        UdpClient receiver = new UdpClient();

        IPAddress multicastAddress = IPAddress.Parse("224.5.6.7");
        int port = 16000;

        // Присоединяемся к multicast группе
        receiver.JoinMulticastGroup(multicastAddress);

        // Связываем с портом
        receiver.Client.Bind(new IPEndPoint(IPAddress.Any, port));

        Console.WriteLine("Multicast получатель запущен. Ожидание сообщений из группы 224.5.6.7...");

        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            byte[] data = receiver.Receive(ref remoteEndPoint);
            string message = Encoding.UTF8.GetString(data);

            Console.WriteLine($"Получено от {remoteEndPoint}: {message}");
        }
    }
}