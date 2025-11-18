// Задание 1: Простой HTTP-клиент
// Цель: Научиться делать HTTP-запросы с помощью HttpClient.
// Код:
using System;
using System.Net.Http;
using System.Threading.Tasks;

class SimpleHttpClient
{
    static async Task Main()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                Console.Write("Введите URL для запроса: ");
                string url = Console.ReadLine();

                // Устанавливаем User-Agent для избежания 403 ошибок
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                HttpResponseMessage response = await client.GetAsync(url);

                Console.WriteLine($"Статус код: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine("Заголовки ответа:");
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"\nПервые 500 символов содержимого:\n{content.Substring(0, Math.Min(500, content.Length))}...");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}