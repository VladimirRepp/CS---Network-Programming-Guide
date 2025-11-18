// Задание 3: Загрузка файла через FTP
// Цель: Научиться работать с FTP-сервером.
// Код:
using System;
using System.IO;
using System.Net;

class FtpDownloader
{
    static void Main()
    {
        // НАСТРОЙКИ (используйте тестовый FTP-сервер)
        string ftpServer = "ftp://ftp.dlptest.com/";
        string username = "dlpuser";
        string password = "rNrKYTX9g7z3RgJRmxWuGHbeu";
        string remoteFileName = "test.txt";
        string localFileName = "downloaded_test.txt";

        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{ftpServer}{remoteFileName}");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            Console.WriteLine("Загрузка файла с FTP-сервера...");

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream fileStream = File.Create(localFileName))
            {
                responseStream.CopyTo(fileStream);
                Console.WriteLine($"Файл загружен! Статус: {response.StatusDescription}");
                Console.WriteLine($"Размер файла: {new FileInfo(localFileName).Length} байт");
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