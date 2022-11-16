using System;
using System.Net;
using System.Threading;

namespace image_downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://73.241.8.159/__live.jpg?&&&";
            Console.WriteLine("Start");
            using (WebClient client = new WebClient())
            {
                while (true)
                {
                    string time = DateTime.Now.ToString("yyyy_dd_MM HH_mm_ss");
                    client.DownloadFileAsync(new Uri(url), @"D:\stalk\" + time + ".jpg");
                    Thread.Sleep(5000);
                }
            }

        }
    }
}
