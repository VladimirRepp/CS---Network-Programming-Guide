// Задание 5: Установка таймаутов на сокете
// Цель: Научиться контролировать время ожидания сетевых операций.
// Код (модификация клиента из Задания 2):
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Устанавливаем таймаут на подключение и операции отправки/получения (5 секунд)
        clientSocket.SendTimeout = 5000;
        clientSocket.ReceiveTimeout = 5000;

        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Loopback, 12345);

        try
        {
            Console.WriteLine("Подключаемся к серверу (таймаут 5с)...");
            // Для Connect таймаут задается через отдельный вызов (или используйте ConnectAsync)
            IAsyncResult result = clientSocket.BeginConnect(serverEndPoint, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(5000, true); // Ждем 5 секунд

            if (!success)
            {
                throw new TimeoutException("Таймаут подключения истек.");
            }
            clientSocket.EndConnect(result);
            Console.WriteLine("Подключено к серверу!");

            // ... остальной код отправки и приема (который теперь тоже имеет таймауты) ...
            string message = "Сообщение с таймаутом!";
            byte[] dataToSend = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(dataToSend);
            Console.WriteLine($"Отправлено: {message}");

            byte[] buffer = new byte[1024];
            int bytesReceived = clientSocket.Receive(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            Console.WriteLine($"Получено: {response}");

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (TimeoutException tex)
        {
            Console.WriteLine($"Таймаут операции: {tex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}