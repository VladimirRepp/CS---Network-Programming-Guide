// Задание 3: Создание и анализ объекта IPEndPoint 
// * Цель: Понять, как формируется сетевая конечная точка. 
// * Код: 
using System;
using System.Net;

class Program
{
    static void Main()
    {
        // Создание IPAddress из строки
        IPAddress ip = IPAddress.Parse("192.168.1.1");
        Console.WriteLine($"IP-адрес: {ip}");

        // Создание IPEndPoint
        IPEndPoint endPoint = new IPEndPoint(ip, 8080); // Порт 8080
        Console.WriteLine($"Конечная точка: {endPoint}");
        Console.WriteLine($"Адрес: {endPoint.Address}, Порт: {endPoint.Port}");

        // Использование специальных констант
        IPEndPoint localEp = new IPEndPoint(IPAddress.Loopback, 9000); // localhost:9000
        Console.WriteLine($"Локальная конечная точка: {localEp}");
    }
}
