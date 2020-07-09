using Microsoft.VisualStudio.TestTools.UnitTesting;
using Requester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requester.Tests
{
	[TestClass()]
	public class RequesterTests
	{
		[TestMethod()]
		public async Task SendTestAsyncGet()
		{
			string method = "get";
			string url = "https://jsonplaceholder.typicode.com/todos/1";
			string body = @"";

			string expectedContent = @"{
  ""userId"": 1,
  ""id"": 1,
  ""title"": ""delectus aut autem"",
  ""completed"": false
}".Replace("\r\n", "\n"); ;

			Requester cm = new Requester();

			RequestResponse response = await cm.Send(method, url, body);

			Assert.AreEqual(200, response.statusCode.Key);
			Assert.AreEqual("OK", response.statusCode.Value);
			Assert.IsNotNull(response.header);
			Assert.AreEqual(expectedContent, response.content);
			Assert.IsNotNull(response.timing);
		}

		[TestMethod()]
		public async Task SendTestAsyncPost()
		{
			string method = "post";
			string url = "https://jsonplaceholder.typicode.com/posts";
			
			Dictionary<string, string> header = new Dictionary<string, string>();
			header.Add("content-type", "application/json");

			string body = @"{""a"":""b""}";

			string expectedContent = @"{
  ""a"": ""b"",
  ""id"": 101
}".Replace("\r\n", "\n"); ;

			Requester cm = new Requester();

			RequestResponse response = await cm.Send(method, url, body, header);

			Assert.AreEqual(201, response.statusCode.Key);
			Assert.AreEqual("Created", response.statusCode.Value);
			Assert.IsNotNull(response.header);
			Assert.AreEqual(expectedContent, response.content);
			Assert.IsNotNull(response.timing);
		}


	}
}