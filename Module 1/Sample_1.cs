// Задание 2: Проверка доступности узла (Ping) 
// * Цель: Продемонстрировать использование System.Net.NetworkInformation.Ping для проверки связи с узлом. 
// * Код:
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Write("Введите IP-адрес или хост для проверки связи: ");
        string host = Console.ReadLine();

        using (Ping ping = new Ping())
        {
            try
            {
                PingReply reply = await ping.SendPingAsync(host, 1000); // Таймаут 1 секунда
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"Ответ от {reply.Address}: время={reply.RoundtripTime}мс");
                }
                else
                {
                    Console.WriteLine($"Не удалось получить ответ: {reply.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
