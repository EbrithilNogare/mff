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
			if(args.Length == 0)
			{
				Console.WriteLine(@"Usage:
  -o	output file
  -t	template
  -m	method
  -u	url
  -c	content
  -h	header
");
				return;
			}

			var argsD = new Dictionary<string, string>() {
				{"-o",argsParseByTag(args, "-o")}, // output file
				{"-t",argsParseByTag(args, "-t")}, // template
				{"-m",argsParseByTag(args, "-m")}, // method
				{"-u",argsParseByTag(args, "-u")}, // url
				{"-c",argsParseByTag(args, "-c")}, // content
				{"-h",argsParseByTag(args, "-h")}, // header
			};
			
			string method = "get", url="", content="";
			Dictionary<string, string> header = new Dictionary<string, string>();

			// load template file
			if (argsD["-t"] != null)
			{
				RequestTemplate requestTemplate;
				using (StreamReader file = File.OpenText(argsD["-t"]))
				{
					JsonSerializer serializer = new JsonSerializer();
					requestTemplate = (RequestTemplate)serializer.Deserialize(file, typeof(RequestTemplate));
				}
				method = requestTemplate.method;
				url = requestTemplate.url;
				content = requestTemplate.content;
				foreach(var headerTemplate in requestTemplate.header)
					header.Add(headerTemplate.Key, headerTemplate.Value);
			}

			// check if args contains information for request
			if (argsD["-m"] != null) method = argsD["-m"];
			if (argsD["-u"] != null) url = argsD["-u"];
			if (argsD["-c"] != null) content = argsD["-c"];
			if (argsD["-h"] != null)
			{
				var headerList = argsD["-h"].Split(';');
				foreach (var headerConcatedPair in headerList)
				{
					var headerPair = headerConcatedPair.Split(':');
					if (headerPair.Length != 2)
						throw new Exception("Header must be in format \"Key1:Value1;Key2:Value2...\"");

					header.Add(headerPair[0], headerPair[1]);
				}
			}
			
			// send request
			Requester cm = new Requester();
			RequestResponse response = await cm.Send(method, url, content, header);

			if(argsD["-o"] != null)
			{
				using (StreamWriter sw = new StreamWriter(argsD["-o"]))
				{
					sw.WriteLine(response.content);
				}
			}
			else
			{
				Console.WriteLine(response.statusCode.Key + " (" + response.statusCode.Value + ")\n");
				Console.WriteLine(response.header);
				Console.WriteLine(response.timing + "ms\n");
				Console.WriteLine("------------------Response------------------");
				Console.WriteLine(response.content);
			}

			Console.ReadKey();
		}

		private static string argsParseByTag(string[] args, string v)
		{
			int index = Array.IndexOf(args, v);
			if (index < 0)
				return null;

			if (index + 1 >= args.Length || args[index+1][0] == '-')
				throw new Exception("Invalid args");

			return args[index+1];
		}
	}   

	public class RequestTemplate
	{
		public string method;
		public string url;
		public KeyValuePair<string,string>[] header;
		public string content;

	}
}