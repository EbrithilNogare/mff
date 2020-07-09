using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Requester;

namespace Requester
{
    class Program
    {
		static async Task Main(string[] args)
		{
			params
			if (Array.IndexOf(args, "-c") > -1)
				if()
			using (StreamReader file = File.OpenText(@"c:\movie.json"))
			{
				JsonSerializer serializer = new JsonSerializer();
				requestTemplate = (RequestTemplate)serializer.Deserialize(file, typeof(RequestTemplate));
			}







			string method = "post";
			string url = "https://jsonplaceholder.typicode.com/todos";
			Dictionary<string, string> header = new Dictionary<string, string>();
			header.Add("content-type", "application/json");
			string body = @"{""a"":""b""}";

			Requester cm = new Requester();

			RequestResponse response = await cm.Send(method, url, body, header);

			Console.WriteLine("header\n" + response.header + "\n\n");
			Console.WriteLine("content\n" + response.content+ "\n\n");
			Console.WriteLine("statusCode\n" + response.statusCode.Key + " (" + response.statusCode.Value + ")" + "\n\n");
			Console.WriteLine("timing\n" + response.timing + "\n\n");



			Console.ReadKey();
		}
    }   

	public class RequestTemplate
	{
		public string method;
		public string url;
		public Dictionary<string,string> header;
		public string content;

	}
}