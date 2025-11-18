// Задание 5: Покидание multicast-группы
// Цель: Показать управление членством в multicast-группах.
// Код:
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class DynamicMulticastClient
{
    static UdpClient client;
    static IPAddress multicastAddress = IPAddress.Parse("224.10.10.10");
    static int port = 19000;
    static bool isInGroup = false;

    static void Main()
    {
        client = new UdpClient();
        client.Client.Bind(new IPEndPoint(IPAddress.Any, port));

        Console.WriteLine("Динамический multicast клиент");
        Console.WriteLine("Команды: 'join' - присоединиться, 'leave' - покинуть, 'exit' - выход");

        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        while (true)
        {
            string command = Console.ReadLine();

            switch (command.ToLower())
            {
                case "join":
                    if (!isInGroup)
                    {
                        client.JoinMulticastGroup(multicastAddress);
                        isInGroup = true;
                        Console.WriteLine("Присоединились к multicast группе 224.10.10.10");
                    }
                    else
                    {
                        Console.WriteLine("Уже в группе!");
                    }
                    break;

                case "leave":
                    if (isInGroup)
                    {
                        client.DropMulticastGroup(multicastAddress);
                        isInGroup = false;
                        Console.WriteLine("Покинули multicast группу 224.10.10.10");
                    }
                    else
                    {
                        Console.WriteLine("Не состоим в группе!");
                    }
                    break;

                case "exit":
                    if (isInGroup)
                        client.DropMulticastGroup(multicastAddress);
                    client.Close();
                    return;
            }
        }
    }

    static void ReceiveMessages()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            try
            {
                byte[] data = client.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);

                if (isInGroup)
                {
                    Console.WriteLine($"Получено multicast сообщение: {message}");
                }
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка приема: {ex.Message}");
            }
        }
    }
}