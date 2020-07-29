using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
				var tl = new TemplateLoader();
				var template = tl.Load(argsD["-t"]);

				method = template.method;

				var uriBuilder = new UriBuilder(template.url);
				var query = HttpUtility.ParseQueryString(uriBuilder.Query);
				foreach (var item in template.parameters)
				{
					if (item.active)
						query[item.key] = item.value;
				}
				uriBuilder.Query = query.ToString();
				url = uriBuilder.ToString();

				content = template.content;
				foreach(var headerTemplate in template.header)
					if(headerTemplate.active)
						header.Add(headerTemplate.key, headerTemplate.value);
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
            try {
			    Requester cm = new Requester();
			    RequestResponse response = await cm.Send(method, url, content, header);

			    if(argsD["-o"] != null)
			    {
				    using (StreamWriter sw = new StreamWriter(argsD["-o"]))
				    {
					    sw.Write(response.content);
				    }
			    }
			    else
			    {
				    Console.WriteLine(response.statusCode.Key + " (" + response.statusCode.Value + ")\n");
				    Console.WriteLine(response.header);
				    Console.WriteLine(response.timing + "ms\n");
				    Console.WriteLine(response.content);
			    }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
                else
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
		/// <summary>
		/// parse args by tag, can be only used on Tuple arguments
		/// </summary>
		/// <param name="args"></param>
		/// <param name="v"></param>
		/// <returns>key, value of param name and its value</returns>
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
}