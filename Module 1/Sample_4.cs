// Задание 5: Простой TCP-клиент для установки соединения 
// * Цель: Показать базовое использование TcpClient для установки TCP-соединения (без обмена данными). 
// * Код:
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Write("Введите хост для подключения: ");
        string host = Console.ReadLine();
        Console.Write("Введите порт: ");
        int port = int.Parse(Console.ReadLine());

        using (TcpClient client = new TcpClient())
        {
            try
            {
                Console.WriteLine($"Попытка подключения к {host}:{port}...");
                await client.ConnectAsync(host, port);
                Console.WriteLine("Подключение установлено успешно!");
                Console.WriteLine($"Локальная конечная точка: {client.Client.LocalEndPoint}");
                Console.WriteLine($"Удаленная конечная точка: {client.Client.RemoteEndPoint}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Ошибка сокета: {ex.SocketErrorCode} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        Console.WriteLine("Соединение закрыто.");
    }
}
