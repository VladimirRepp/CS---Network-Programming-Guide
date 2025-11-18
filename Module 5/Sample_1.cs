// Задание 2: Отправка email через SMTP
// Цель: Научиться отправлять email с помощью SmtpClient.
// Код:
using System;
using System.Net;
using System.Net.Mail;

class EmailSender
{
    static void Main()
    {
        // НАСТРОЙКИ (замените на реальные данные)
        string smtpServer = "smtp.gmail.com";
        int port = 587;
        string username = "your-email@gmail.com";
        string password = "your-app-password"; // Используйте пароль приложения
        bool enableSsl = true;

        try
        {
            using (SmtpClient smtpClient = new SmtpClient(smtpServer, port))
            {
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.EnableSsl = enableSsl;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(username);
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Тестовое письмо из C#";
                mailMessage.Body = "Это тестовое письмо, отправленное через SmtpClient в .NET.";
                mailMessage.IsBodyHtml = false;

                Console.WriteLine("Отправка письма...");
                smtpClient.Send(mailMessage);
                Console.WriteLine("Письмо успешно отправлено!");
            }
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"SMTP ошибка: {ex.StatusCode} - {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}