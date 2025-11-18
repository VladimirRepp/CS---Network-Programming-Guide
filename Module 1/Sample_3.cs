// Задание 4: Определение типа адреса (IPv4/IPv6) 
// * Цель: Научиться работать с семейством адресов. 
// * Код:
using System;
using System.Net;

class Program
{
    static void Main()
    {
        Console.Write("Введите IP-адрес: ");
        string input = Console.ReadLine();

        if (IPAddress.TryParse(input, out IPAddress address))
        {
            Console.WriteLine($"Введенный адрес: {address}");
            Console.WriteLine($"Семейство адресов: {address.AddressFamily}");
            Console.WriteLine($"Это IPv4? {address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork}");
            Console.WriteLine($"Это IPv6? {address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6}");
            Console.WriteLine($"Это адрес обратной петли (localhost)? {IPAddress.IsLoopback(address)}");
        }
        else
        {
            Console.WriteLine("Неверный формат IP-адреса.");
        }
    }
}
