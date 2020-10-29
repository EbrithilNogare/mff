using Microsoft.VisualStudio.TestTools.UnitTesting;
using Requester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
			string url = "https://jsonplaceholder.typicode.com/to"+"dos/1"; // splitted because of keyword to_do
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

		[TestMethod()]
		public async Task IvalidURL()
		{
			string method = "post";
			string url = "notAValidURL";
			Requester cm = new Requester();

			Dictionary<string, string> header = new Dictionary<string, string>();
			header.Add("content-type", "application/json");

			string body = @"{""a"":""b""}";

			try
			{
				RequestResponse response = await cm.Send(method, url, body, header);
				Assert.Fail();
			}
			catch (HttpRequestException)
			{
				// correct exception
				return;
			}
			catch (Exception)
			{
				Assert.Fail();
			}
		}

		[TestMethod()]
		public async Task PartialyValidURL()
		{
			string method = "post";
			string url = "jsonplaceholder.typicode.com/posts";

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


		[TestMethod()]
		public void SetHeadersTest()
		{
			Requester cm = new Requester();
			Dictionary<string, string> headerParams = new Dictionary<string, string>() { { "Hname", "Hvalue" } };
			StringContent requestBody = new StringContent("");
			cm.SetHeaders(headerParams, requestBody);
			Assert.AreEqual("Hvalue", requestBody.Headers.GetValues("Hname").ToList()[0]);
		}

		[TestMethod()]
		public void SetHeadersTestTwoSame()
		{
			Requester cm = new Requester();
			Dictionary<string, string> headerParams = new Dictionary<string, string>() { { "Hname", "AAA" } };
			StringContent requestBody = new StringContent("");
			cm.SetHeaders(headerParams, requestBody);
			headerParams["Hname"] = "BBB";
			cm.SetHeaders(headerParams, requestBody);
			Assert.AreEqual("BBB", requestBody.Headers.GetValues("Hname").ToList()[0]);
		}

		[TestMethod()]
		public void ApplyParamsToUrlTestEmpty()
		{
			Requester cm = new Requester();
			string url = "https://reqbin.com/echo/get/json";
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			var newUrl = cm.ApplyParamsToUrl(url, parameters);
			Assert.AreEqual("https://reqbin.com:443/echo/get/json", newUrl);
		}

		[TestMethod()]
		public void ApplyParamsToUrlTestOne()
		{
			Requester cm = new Requester();
			string url = "https://reqbin.com/echo/get/json";
			Dictionary<string, string> parameters = new Dictionary<string, string>() { { "p1", "P1" }};
			var newUrl = cm.ApplyParamsToUrl(url, parameters);
			Assert.AreEqual("https://reqbin.com:443/echo/get/json?p1=P1", newUrl);
		}

		[TestMethod()]
		public void ApplyParamsToUrlTestTwo()
		{
			Requester cm = new Requester();
			string url = "https://reqbin.com/echo/get/json";
			Dictionary<string, string> parameters = new Dictionary<string, string>() { { "p1", "P1" }, { "p2", "P2" } };
			var newUrl = cm.ApplyParamsToUrl(url, parameters);
			Assert.AreEqual("https://reqbin.com:443/echo/get/json?p1=P1&p2=P2", newUrl);
		}

		[TestMethod()]
		public void ApplyParamsToUrlTestTwoSame()
		{
			Requester cm = new Requester();
			string url = "https://reqbin.com/echo/get/json";
			Dictionary<string, string> parameters = new Dictionary<string, string>() { { "p1", "P1" } };
			var newUrl = cm.ApplyParamsToUrl(url, parameters);
			parameters["p1"] = "P2";
			newUrl = cm.ApplyParamsToUrl(newUrl, parameters);
			Assert.AreEqual("https://reqbin.com:443/echo/get/json?p1=P2", newUrl);
		}

		[TestMethod()]
		public void ApplyParamsToUrlTestForcedPort()
		{
			Requester cm = new Requester();
			string url = "https://reqbin.com:80/echo/get/json";
			Dictionary<string, string> parameters = new Dictionary<string, string>() { { "p1", "P1" }};
			var newUrl = cm.ApplyParamsToUrl(url, parameters);
			Assert.AreEqual("https://reqbin.com:80/echo/get/json?p1=P1", newUrl);
		}

		[TestMethod()]
		public void ApplyParamsToUrlTestForcedPort2()
		{
			Requester cm = new Requester();
			string url = "https://reqbin.com:50080/echo/get/json";
			Dictionary<string, string> parameters = new Dictionary<string, string>() { { "p1", "P1" }};
			var newUrl = cm.ApplyParamsToUrl(url, parameters);
			Assert.AreEqual("https://reqbin.com:50080/echo/get/json?p1=P1", newUrl);
		}
	}
}