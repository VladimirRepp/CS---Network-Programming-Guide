// Задание 1: Разрешение доменного имени 
// * Цель: Научиться использовать класс Dns для получения IP-адресов по имени хоста. 
// * Код:
using System;
using System.Net;

class Program
{
    static void Main()
    {
        Console.Write("Введите доменное имя (например, google.com): ");
        string hostname = Console.ReadLine();

        try
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(hostname);
            Console.WriteLine($"IP-адреса для {hostname}:");
            foreach (IPAddress address in hostInfo.AddressList)
            {
                Console.WriteLine($"  {address}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
