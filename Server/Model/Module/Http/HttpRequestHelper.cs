
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public  static class HttpRequestHelper
    {
        public const string GET = "GET";
        public const string POST = "POST";
        private static readonly string CONTENTTYPE_FORM = "application/x-www-form-urlencoded";
        private static readonly string CONTENTTYPE_JSON = "application/json";

        #region 创建Http Post请求

        private static HttpWebRequest CreatePostHttpWebRequest(string url, string postData, string contentType)
        {
            HttpWebRequest postRequest = WebRequest.Create(url) as HttpWebRequest;

            //持久连接
            postRequest.KeepAlive = false;

            postRequest.Timeout = 5000000;

            postRequest.Method = POST;

            //告诉服务端消息主体是序列化后的JSON字符串
            postRequest.ContentType = contentType;

            postRequest.ContentLength = postData.Length;

            postRequest.AllowWriteStreamBuffering = false;

            StreamWriter writer = new StreamWriter(postRequest.GetRequestStream(), Encoding.UTF8);

            writer.Write(postData);

            writer.Flush();

            return postRequest;
        }

        #endregion

        #region 发送Http Post异步请求

        public static async Task<string> SendPostRequestAsync(string url, string postData)
        {
            string respStr = string.Empty;

            try
            {
                HttpWebRequest postRequest = CreatePostHttpWebRequest(url, postData, CONTENTTYPE_FORM);

                HttpWebResponse postResponse = await postRequest.GetResponseAsync() as HttpWebResponse;

                respStr = ConvertHttpResponseToStr(postResponse, POST);
                Log.Debug(respStr);
            }
            catch (System.Exception ex)
            {
                respStr = ex.Message;

                Log.Error(ex.Message);
            }

            // return JsonHelper.FromJson<TResult>(respStr);
            return respStr;
        }

        #endregion

        #region  HttpResponse转Json字符串

        private static string ConvertHttpResponseToStr(HttpWebResponse response, string requestType)
        {
            string responseResult = "";

            string encoding = "UTF-8";

            if (string.Equals(requestType, POST, System.StringComparison.OrdinalIgnoreCase))
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

        #endregion
        
    }
}
