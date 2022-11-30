using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Selenium_API.Model;
using Selenium_API.Model.JsonModel;
using Selenium_API.Model.SendModel;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_API
{
    [TestClass]
    public class UnitTest1
    {

        private string geturl = "https://reqres.in/api/users?page=2";
        private string sendurl = "https://reqres.in/api/users";

        [TestMethod]
        public void GET_with_URL()
        {
            HttpClient httpClient = new HttpClient();

            var xyz = httpClient.GetAsync(geturl);

            httpClient.Dispose();

        }

        [TestMethod]
        public void GET_with_URI()
        {
            HttpClient httpClient = new HttpClient();

            Uri geturi = new Uri(geturl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(geturi);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());
            httpClient.Dispose();
        }

        [TestMethod]
        public void GET_with_URI_ExtractingStatusCode()
        {
            HttpClient httpClient = new HttpClient();

            Uri geturi = new Uri(geturl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(geturi);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            HttpStatusCode httpStatusCode =   httpResponseMessage.StatusCode;
            Console.WriteLine("Status : " + httpStatusCode);
            Console.WriteLine("Status Code: "+ (int)httpStatusCode);


            httpClient.Dispose();
        }

        [TestMethod]
        public void GET_with_URI_ExtractingMessage()
        {
            HttpClient httpClient = new HttpClient();

            Uri geturi = new Uri(geturl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(geturi);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            HttpContent httpContent = httpResponseMessage.Content;
            Task<string> responseData = httpContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            httpClient.Dispose();
        }

        [TestMethod]
        public void GET_with_URI_Changing_Header_Format()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders httpRequestHeaders = httpClient.DefaultRequestHeaders;
            httpRequestHeaders.Add("Accept", "application/json");


            Uri geturi = new Uri(geturl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(geturi);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            HttpContent httpContent = httpResponseMessage.Content;
            Task<string> responseData = httpContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            httpClient.Dispose();
        }


        [TestMethod]
        public void GET_with_URI_Changing_Header_Format_Using_Accept()
        {
            MediaTypeWithQualityHeaderValue jsonheadaer = new MediaTypeWithQualityHeaderValue("application/json");

            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders httpRequestHeaders = httpClient.DefaultRequestHeaders;
            httpRequestHeaders.Accept.Add(jsonheadaer);


            Uri geturi = new Uri(geturl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(geturi);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            HttpContent httpContent = httpResponseMessage.Content;
            Task<string> responseData = httpContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            httpClient.Dispose();
        }

        [TestMethod]
        public void GET_with_URI_Using_SendAsync()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri(geturl);
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Add("Accept","application/json");

            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

             
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            HttpContent httpContent = httpResponseMessage.Content;
            Task<string> responseData = httpContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            httpClient.Dispose();
        }


        [TestMethod]
        public void GET_by_USING_Method()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(geturl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    
                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        HttpContent httpContent = httpResponseMessage.Content;
                        Task<string> responseData = httpContent.ReadAsStringAsync();
                        string data = responseData.Result;
                        Console.WriteLine(data);
                    }

                }
            }
        }

        [TestMethod]
        public void GET_by_USING_Method_Custom_Class()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(geturl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");


                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        HttpContent httpContent = httpResponseMessage.Content;
                        Task<string> responseData = httpContent.ReadAsStringAsync();
                        string data = responseData.Result;
                        HttpStatusCode statusCode = httpResponseMessage.StatusCode ;
                        //Console.WriteLine(data);


                        RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);
                        Console.WriteLine(restResponse.ToString());
                    }
                     
                }
            }
        }

        [TestMethod]
        public void GET_DeserilizationOfJsonResponse()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(geturl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");


                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        HttpContent httpContent = httpResponseMessage.Content;
                        Task<string> responseData = httpContent.ReadAsStringAsync();
                        //string data = responseData.Result;
                        HttpStatusCode statusCode = httpResponseMessage.StatusCode;



                        RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);
                        //Console.WriteLine(restResponse.ToString());

                        //Datum datum =  JsonConvert.DeserializeObject<Datum>(restResponse.responseContent);
                        //Console.WriteLine(datum.ToString());

                        Root root = JsonConvert.DeserializeObject<Root>(restResponse.responseContent);
                        Console.WriteLine(root.ToString());

                        Support support = JsonConvert.DeserializeObject<Support>(restResponse.responseContent);
                        Console.WriteLine(support.ToString());


                    }

                }
            }
        }

        [TestMethod]
        public void GET_with_simpletype_deserilization()
        {
            HttpClient httpClient = new HttpClient();

            Uri geturi = new Uri(geturl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(geturi);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            HttpContent httpContent = httpResponseMessage.Content;
            Task<string> responseData = httpContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            Root root = JsonConvert.DeserializeObject<Root>(responseData.Result);
            Console.WriteLine(root.ToString());

            httpClient.Dispose();
        }


        [TestMethod]
        public void POST_Start()
        {
            HttpClient httpClient = new HttpClient();

            Uri senduri = new Uri(sendurl);
            var jdata = new Application() { name = "iqoo" , job ="performance" };
            var jdataserialized=  JsonConvert.SerializeObject(jdata);
            var stringContent = new StringContent(jdataserialized,Encoding.UTF8,"application/json");


            Task<HttpResponseMessage> httpResponse = httpClient.PostAsync(senduri,stringContent);


            //Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(geturi);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;

            var s =httpResponseMessage.StatusCode;

            HttpContent httpContent = httpResponseMessage.Content;
            Task<string> responseData = httpContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            Root1 root = JsonConvert.DeserializeObject<Root1>(responseData.Result);
            Console.WriteLine(root.ToString());

            httpClient.Dispose();
        }












































        //
    }
}
