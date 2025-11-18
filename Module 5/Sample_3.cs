// Задание 4: HTTP POST-запрос с JSON
// Цель: Научиться отправлять POST-запросы с JSON-данными.
// Код:
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class HttpPostClient
{
    public class PostData
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
    }

    static async Task Main()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // JSONPlaceholder - тестовый API
                string url = "https://jsonplaceholder.typicode.com/posts";

                // Создаем данные для отправки
                var postData = new PostData
                {
                    Title = "Test Post from C#",
                    Body = "This is a test post sent via HttpClient",
                    UserId = 1
                };

                // Сериализуем в JSON
                string json = JsonSerializer.Serialize(postData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine("Отправка POST-запроса...");
                Console.WriteLine($"Данные: {json}");

                HttpResponseMessage response = await client.PostAsync(url, content);

                Console.WriteLine($"Статус код: {(int)response.StatusCode} {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ответ сервера:\n{responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}