using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Requester
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            string url = "https://request.urih.com/";
            string type = "POST";
            string header = "";

            Comunicator cm = new Requester.Comunicator();

            cm.Send(url, type, header);

            
            Console.ReadKey();
        }

        
    }

    public class Comunicator
    {
        private static readonly HttpClient client = new HttpClient();

        public Comunicator()
        {

        }

        public async Task<string> Send(string url, string type, string header)
        {
            if (!(url.Contains("http://") || url.Contains("https://")))
                url = "https://" + url;
            var values = new Dictionary<string, string> //TODO remove it
                {
                   { "thing1", "hello" },
                   { "thing2", "world" }
                };

            var content = new FormUrlEncodedContent(values);
            //Console.WriteLine(await content.ReadAsStringAsync());
            HttpResponseMessage response = null;
            switch (type)
            {
                case "GET":
                    response = await client.GetAsync(url);
                    break;
                case "POST":
                    response = await client.PostAsync(url,content);
                    break;
            }

            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

    }
   
}