using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YusX.Core.Utilities
{
    /// <summary>
    /// HTTP 请求客户端帮助类
    /// </summary>
    public class HttpClientHelper
    {
        /// <summary>
        /// 发送 GET 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="timeout">请求超时，单位毫秒</param>
        /// <param name="headers">请求头信息</param>
        /// <returns></returns>
        public static Task<string> GetAsync(string url, int timeout = 30, Dictionary<string, string> headers = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeout;
                request.Method = "GET";

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers[header.Key] = header.Value;
                    }
                }

                return ReadResponseSteam(request);
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message);
            }
        }

        /// <summary>
        /// 发送 POST 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">请求数据，将使用 UTF-8 编码发送</param>
        /// <param name="contentType">内容类型，例如 application/json</param>
        /// <param name="timeout">请求超时，单位毫秒</param>
        /// <param name="headers">请求头信息</param>
        /// <returns></returns>
        public static Task<string> PostAsync(
            string url,
            string postData = null,
            string contentType = null,
            int timeout = 30,
            Dictionary<string, string> headers = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.Method = "POST";

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                request.ContentType = contentType;
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            try
            {
                var dataBytes = Encoding.UTF8.GetBytes(postData ?? "");
                using (var sendStream = request.GetRequestStream())
                {
                    sendStream.Write(dataBytes, 0, dataBytes.Length);
                }

                return ReadResponseSteam(request);
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message);
            }
        }

        /// <summary>
        /// 读取响应流并转换为字符串内容
        /// </summary>
        /// <param name="request">HTTP请求对象</param>
        /// <returns></returns>
        protected static Task<string> ReadResponseSteam(HttpWebRequest request)
        {
            using var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream, Encoding.UTF8);
            return streamReader.ReadToEndAsync();
        }
    }
}
