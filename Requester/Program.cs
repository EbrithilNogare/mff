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
            NewMethod();
            Console.ReadKey();
        }

        private static async void NewMethod()
        {
            var values = new Dictionary<string, string>
                {
                   { "thing1", "hello" },
                   { "thing2", "world" }
                };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(response);
            Console.ReadKey();
        }
        
    }

    
}
