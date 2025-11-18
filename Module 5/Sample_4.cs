// Задание 5: Получение списка файлов по FTP
// Цель: Научиться получать список файлов с FTP-сервера.
// Код:
using System;
using System.IO;
using System.Net;

class FtpFileLister
{
    static void Main()
    {
        string ftpServer = "ftp://ftp.dlptest.com/";
        string username = "dlpuser";
        string password = "rNrKYTX9g7z3RgJRmxWuGHbeu";

        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.KeepAlive = false;

            Console.WriteLine("Получение списка файлов с FTP-сервера...");

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string directoryListing = reader.ReadToEnd();
                Console.WriteLine("Содержимое директории:");
                Console.WriteLine(directoryListing);

                Console.WriteLine($"Статус: {response.StatusDescription}");
            }
        }
        catch (WebException ex)
        {
            FtpWebResponse response = (FtpWebResponse)ex.Response;
            Console.WriteLine($"FTP ошибка: {response?.StatusDescription ?? ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}