using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Requester
{
	public class Requester
	{
		private readonly HttpClient client;

		public Requester()
		{
			client = new HttpClient();
		}

		public async Task<RequestResponse> Send(string method, string url, string body, Dictionary<string, string> headerParams = null)
		{
			// URL
			if (!ValidateURL(url))
				throw new Exception("Invalid URL");


			// Authorization
			// todo

			// Content
			StringContent requestBody = new StringContent(body);

			// Headers
			if (headerParams != null)
				SetHeaders(headerParams, requestBody);

			// Send request
			HttpResponseMessage response;
			switch (method.ToUpper())
			{
				case "POST":
					response = await client.PostAsync(url, requestBody);
					break;
				case "GET":
					response = await client.GetAsync(url);
					break;
				default:
					throw new Exception("Invalid Method");
			}
			
			// Parse response
			RequestResponse output = new RequestResponse();
			output.content = await response.Content.ReadAsStringAsync();
			output.header = response.Headers.ToString();
			output.statusCode = new KeyValuePair<int, string>((int)response.StatusCode, response.StatusCode.ToString());
			output.timing = "0";

			return output;
		}

		private void Post(string body)
		{

		}
		private void Get(string body)
		{

		}

		private void SetHeaders(Dictionary<string, string> headerParams, StringContent content)
		{
			foreach (var param in headerParams)
			{
				// because content has default header
				if (content.Headers.Contains(param.Key))
				{
					content.Headers.Remove(param.Key);
					content.Headers.Add(param.Key, param.Value);
				}
				else
				{
					content.Headers.Add(param.Key, param.Value);
				}
			}
		}


		private bool ValidateURL(string url)
		{
			// todo
			return true;
		}
	}

	public struct RequestResponse
	{
		public KeyValuePair<int, string> statusCode;
		public string header;
		public string content;
		public string timing;
	}
}
