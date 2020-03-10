using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Common.Utilities
{
    public class WebServiceConsumer
    {
        public static T Invoke<T>(string url, object data, MethodType method, RequestOptions options = null)
        {
            string json = null;
            if (method == MethodType.Post || method == MethodType.Put)
                if (data != null)
                    json = Serialize(data);

            var result = InvokeJson(url, json, method, options);
            var resultObject = Deserialize<T>(result);
            return resultObject;
        }

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static T Deserialize<T>(string result)
        {
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static string InvokeJson(string url, object data, MethodType method, RequestOptions options = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            if (method != MethodType.NotSet)
                request.Method = method.ToString();
            else
                request.Method = "POST";

            request.ContentType = !string.IsNullOrWhiteSpace(options?.ContentType) ? options.ContentType : "application/json";

            if (!string.IsNullOrWhiteSpace(options?.Authorization))
                request.Headers.Add("Authorization", options.Authorization);
            if (!string.IsNullOrWhiteSpace(options?.STH))
                request.Headers.Add("STH", options.STH);

            if (options?.Headers != null)
                foreach (var header in options.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

            if (method == MethodType.Post || method == MethodType.Put)
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    if (data != null)
                    {
                        streamWriter.Write(data);
                    }
                }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                var result = streamReader.ReadToEnd();
                //var resultObject = JsonConvert.DeserializeObject<T>(result);
                return result;
            }
        }


        private static T Get<T>(string url, RequestOptions options)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            if (!string.IsNullOrWhiteSpace(options?.ContentType))
                request.ContentType = options.ContentType;

            if (!string.IsNullOrWhiteSpace(options?.Authorization))
                request.Headers.Add("Authorization", options.Authorization);

            if (options?.Headers != null)
                foreach (var header in options.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                var result = reader.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(result))
                {
                    var resultObject = JsonConvert.DeserializeObject<T>(result);
                    return resultObject;
                }
            }
            return default(T);
        }

    }

    public class RequestOptions
    {
        public string ContentType { get; set; }
        public string Authorization { get; set; }
        public string STH { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }

    public class Token
    {
        public string token { get; set; }
        public string sth { get; set; }
    }

    public class Model<T>
    {
        public bool succeed { get; set; }
        public T data { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public string userMessage { get; set; }
        public int type { get; set; }
    }


    public enum MethodType
    {
        NotSet = 0,
        Get = 1,
        Post = 2,
        Put = 3,
        Delete = 4
    }
}
