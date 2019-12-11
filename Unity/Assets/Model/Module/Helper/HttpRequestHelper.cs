using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 请求方法
    /// </summary>
    public static class HttpRequestMethod
    {
        public const string GET = "GET";
        public const string POST = "POST";
    }

    /// <summary>
    /// HTTP请求辅助类
    /// </summary>
    public static class HttpRequestHelper
    {
        private static readonly string CONTENTTYPE_FORM = "application/x-www-form-urlencoded";
        private static readonly string CONTENTTYPE_JSON = "application/json";

        /// <summary>
        /// 发送HTTP Get同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns>响应报文的字符串</returns>
        public static string SendGetRequest(string url)
        {
            string respStr = string.Empty;
            try
            {
                HttpWebRequest httpWebRequest = CreateGetHttpWebRequest(url);

                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;

                respStr = ConvertHttpResponseToStr(httpWebResponse, HttpRequestMethod.GET);
            }
            catch (System.Exception ex)
            {

                respStr = ex.Message;
                Log.Error(ex.Message);
            }

            return respStr;
        }

        /// <summary>
        /// 发送HTTP Get 同步请求
        /// </summary>
        /// <typeparam name="TResult">响应JSON字符串转成的对象的类型</typeparam>
        /// <param name="url"></param>
        /// <returns>响应JSON字符串转成的对象</returns>
        public static TResult SendGetRequest<TResult>(string url)
        {
            string respStr = string.Empty;

            try
            {
                HttpWebRequest httpWebRequest = CreateGetHttpWebRequest(url);
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                respStr = ConvertHttpResponseToStr(httpWebResponse, HttpRequestMethod.GET);
            }
            catch (System.Exception ex)
            {
                respStr = ex.Message;
                Log.Error(ex.Message);
            }

            return JsonHelper.FromJson<TResult>(respStr);
        }

        /// <summary>
        /// 发送HTTP Get异步请求
        /// </summary>
        /// <typeparam name="TResult">响应JSON字符串转成的对象的类型</typeparam>
        /// <param name="url"></param>
        /// <returns>响应JSON字符串转成的对象</returns>
        public static async Task<TResult> SendGetRequestAsync<TResult>(string url)
        {
            string respStr = string.Empty;

            try
            {
                HttpWebRequest httpWebRequest = CreateGetHttpWebRequest(url);

                HttpWebResponse httpWebResponse = await httpWebRequest.GetResponseAsync() as HttpWebResponse;

                respStr = ConvertHttpResponseToStr(httpWebResponse, HttpRequestMethod.GET);
            }
            catch (System.Exception ex)
            {

                respStr = ex.Message;
                Log.Error(ex.Message);
            }

            return JsonHelper.FromJson<TResult>(respStr);
        }

        //////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 发送HTTP Post同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postJsonData">请求的参数(JSON字符串)</param>
        /// <returns></returns>
        public static string SendPostRequest(string url, string postJsonData)
        {
            string respStr = string.Empty;

            try
            {
                HttpWebRequest postRequest = CreatePostHttpWebRequest(url, postJsonData,CONTENTTYPE_JSON);

                HttpWebResponse postResponse = postRequest.GetResponse() as HttpWebResponse;

                respStr = ConvertHttpResponseToStr(postResponse, HttpRequestMethod.POST);
            }
            catch (System.Exception ex)
            {

                respStr = ex.Message;

                Log.Error(ex.Message);

            }

            return respStr;
        }

        /// <summary>
        /// 发送Http Post同步请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="postJsonData">post请求的参数(JSON字符串)</param>
        /// <returns></returns>
        public static TResult SendPostRequest<TResult>(string url, string postJsonData)
        {
            string respStr = string.Empty;

            try
            {
                HttpWebRequest postRequest = CreatePostHttpWebRequest(url, postJsonData,CONTENTTYPE_JSON);

                HttpWebResponse postResponse = postRequest.GetResponse() as HttpWebResponse;

                respStr = ConvertHttpResponseToStr(postResponse, HttpRequestMethod.POST);
            }
            catch (System.Exception ex)
            {

                respStr = ex.Message;

                Log.Error(ex.Message);
            }

            return JsonHelper.FromJson<TResult>(respStr);
        }

        /// <summary>
        /// 发送Http Post异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post请求的参数(JSON字符串)</param>
        /// <returns>响应的JSON字符串</returns>
        public static async Task<string> SendPostRequestAsync(string url, string postData)
        {
            string respStr = string.Empty;

            try
            {
                HttpWebRequest postRequest = CreatePostHttpWebRequest(url, postData,CONTENTTYPE_FORM);

                HttpWebResponse postResponse = await postRequest.GetResponseAsync() as HttpWebResponse;

                respStr = ConvertHttpResponseToStr(postResponse, HttpRequestMethod.POST);
            }
            catch (System.Exception ex)
            {
                respStr = ex.Message;

                Log.Error(ex.Message);
            }

            return respStr;
        }

        /// <summary>
        /// 发送Http Post异步请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="postData">post请求的参数(JSON字符串)</param>
        /// <returns>响应的JSON字符串转成的对象</returns>
        public static async Task<TResult> SendPostRequestAsync<TResult>(string url, string postData)
        {
            string respStr = string.Empty;

            try
            {
                
                HttpWebRequest postRequest = CreatePostHttpWebRequest(url, postData, CONTENTTYPE_FORM);

                HttpWebResponse postResponse = await postRequest.GetResponseAsync() as HttpWebResponse;

                respStr = ConvertHttpResponseToStr(postResponse, HttpRequestMethod.POST);
                
            }
            catch (System.Exception ex)
            {
                respStr = ex.Message;

                Log.Error(ex.Message);
            }

            return JsonHelper.FromJson<TResult>(respStr);
        }

        /// <summary>
        /// 发送Http Post异步请滶
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="postData">post请求的参数</param>
        /// <returns>响应的JSON字符串转成的对象</returns>
        public static async Task<TResult> SendPostRequestAsync<TResult>(string url, object postData)
        {
            string respStr = string.Empty;

            try
            {
                Debug.Log(JsonHelper.ToJson(postData));
                HttpWebRequest postRequest = CreatePostHttpWebRequest(url, JsonHelper.ToJson(postData),CONTENTTYPE_JSON);

                HttpWebResponse postResponse = await postRequest.GetResponseAsync() as HttpWebResponse;

                respStr = ConvertHttpResponseToStr(postResponse, HttpRequestMethod.POST);
            }
            catch (System.Exception ex)
            {
                respStr = ex.Message;
                Log.Error(ex.Message);
            }

            return JsonHelper.FromJson<TResult>(respStr);
        }

        ////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 创建HTTP Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static HttpWebRequest CreateGetHttpWebRequest(string url)
        {
            HttpWebRequest httpWebRequest = HttpWebRequest.Create(url) as HttpWebRequest;

            httpWebRequest.Method = HttpRequestMethod.GET;

            httpWebRequest.Timeout = 5000;

            //设置页面内容是html,编码格式是utf-8
            httpWebRequest.ContentType = "text/html;charset=UTF-8";

            //似乎只有IE不支持压缩
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return httpWebRequest;
        }

        /// <summary>
        /// 创建Http Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static HttpWebRequest CreatePostHttpWebRequest(string url, string postData,string contentType)
        {
            HttpWebRequest postRequest = HttpWebRequest.Create(url) as HttpWebRequest;

            //持久连接
            postRequest.KeepAlive = false;

            postRequest.Timeout = 5000;

            postRequest.Method = HttpRequestMethod.POST;

            //告诉服务端消息主体是序列化后的JSON字符串
            postRequest.ContentType = contentType;

            postRequest.ContentLength = postData.Length;

            postRequest.AllowWriteStreamBuffering = false;

            StreamWriter writer = new StreamWriter(postRequest.GetRequestStream(), Encoding.ASCII);

            writer.Write(postData);

            writer.Flush();

            return postRequest;
        }

        /// <summary>
        /// HttpResponse转Json字符串
        /// </summary>
        /// <param name="response"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        private static string ConvertHttpResponseToStr(HttpWebResponse response, string requestType)
        {
            string responseResult = "";

            string encoding = "UTF-8";

            if (string.Equals(requestType, HttpRequestMethod.POST, System.StringComparison.OrdinalIgnoreCase))
            {
                encoding = response.ContentEncoding;

                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8";
                }
            }

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
            {
                responseResult = reader.ReadToEnd();
            }

            return responseResult;
        }

        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 模拟表单提交
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="postData"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string PostForm(string requestUri, NameValueCollection postData, CookieContainer cookie)
        {
            HttpWebRequest request = WebRequest.CreateHttp(requestUri);

            request.Method = "post";

            request.ContentType = "application/x-www-form-urlencoded";

            request.CookieContainer = cookie;

            StringBuilder stringBuilder = new StringBuilder();

            foreach (string key in postData.Keys)
            {
                stringBuilder.AppendFormat("&{0}={1}", key, postData.Get(key));
            }

            byte[] buffer = Encoding.UTF8.GetBytes(stringBuilder.ToString().Trim('&'));

            Stream requestStream = request.GetRequestStream();

            requestStream.Write(buffer, 0, buffer.Length);

            requestStream.Close();

            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

            return reader.ReadToEnd();
        }

    }

}
