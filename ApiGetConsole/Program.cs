using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ApiGetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://jsonplaceholder.typicode.com/guide.html - Test API

            bool useCredentials = false;
            //GET from API
            string url = @"https://jsonplaceholder.typicode.com/posts/1/comments";            
            var result = GetFromApi(url, useCredentials);
            Console.WriteLine(result);

            //post to API
            Console.WriteLine("\r\n\r\nNow try an HTTP POST\r\n\r\n");
            result = string.Empty;
            url = @"https://jsonplaceholder.typicode.com/posts";
            result = PostToApi(url, useCredentials);
            Console.WriteLine(result);
        }

        public static string GetFromApi(string url, bool useCredentials)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "Get";
            if (useCredentials)
            {
                request.Credentials = new NetworkCredential("USERNAME", "PASSWORD");
            }
            HttpWebResponse response = null;
            response = (HttpWebResponse)request.GetResponse();
            string strResult = string.Empty;
            //using (Stream stream = response.GetResponseStream())
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                //StreamReader sr = new StreamReader(stream);
                strResult = sr.ReadToEnd();
                sr.Close();
            }
            var posts = JsonConvert.DeserializeObject<List<Post>>(strResult);
            return strResult;
        }

        public static string PostToApi(string url, bool useCredentials)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            if (useCredentials)
            {
                request.Credentials = new NetworkCredential("USERNAME", "PASSWORD");
            }
            HttpWebResponse response = null;
            string postData = "{\"title\": \"Foo\",\"body\":\"test body\", \"userId\":\"50\"}";
            string strResult = string.Empty;
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(postData);
                sw.Flush();
                sw.Close();

                response = (HttpWebResponse)request.GetResponse();
                using(var sr = new StreamReader(response.GetResponseStream()))
                {
                    strResult = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return strResult;
        }



    }
}
