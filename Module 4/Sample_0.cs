// Задание 1: UDP Broadcast - отправитель и получатель
// Цель: Показать основы широковещательной рассылки.
// Код (Broadcast Sender):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class BroadcastSender
{
    static void Main()
    {
        Socket senderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        senderSocket.EnableBroadcast = true;

        IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, 15000); // Порт 15000

        Console.WriteLine("Broadcast отправитель запущен. Введите сообщения:");

        int messageCount = 0;
        while (true)
        {
            string message = $"Broadcast сообщение #{++messageCount} от {DateTime.Now:T}";
            byte[] data = Encoding.UTF8.GetBytes(message);

            senderSocket.SendTo(data, broadcastEndPoint);
            Console.WriteLine($"Отправлено: {message}");

            Thread.Sleep(3000); // Отправляем каждые 3 секунды
        }
    }
}

// Код (Broadcast Receiver):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class BroadcastReceiver
{
    static void Main()
    {
        Socket receiverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Связываем сокет с портом, на который приходят broadcast сообщения
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 15000);
        receiverSocket.Bind(localEndPoint);

        Console.WriteLine("Broadcast получатель запущен. Ожидание сообщений...");

        byte[] buffer = new byte[1024];
        EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            int bytesReceived = receiverSocket.ReceiveFrom(buffer, ref senderEndPoint);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

            Console.WriteLine($"Получено от {senderEndPoint}: {message}");
        }
    }
}