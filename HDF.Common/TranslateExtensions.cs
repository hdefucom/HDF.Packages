using System;
using System.IO;
using System.Net;
#if !NET40
using System.Net.Http;
#endif
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HDF.Common
{
    /// <summary>
    /// 翻译拓展
    /// </summary>
    public static class TranslateExtensions
    {

#pragma warning disable IDE1006,CS1591 // 命名样式，注释
        /// <summary>
        /// 百度--通用翻译api实体
        /// </summary>
        public class BaiduTranslateApiResult
        {
            public string? from { get; set; }
            public string? to { get; set; }
            public TranslateResult[]? trans_result { get; set; }
            public string? error_code { get; set; }


            public class TranslateResult
            {
                public string? src { get; set; }
                public string? dst { get; set; }
            }
        }

        /// <summary>
        /// 百度--语种识别api实体
        /// </summary>
        public class BaiduLanguageApiResult
        {
            public string? error_code { get; set; }
            public string? error_msg { get; set; }
            public LanguageResult? data { get; set; }


            public class LanguageResult
            {
                public string? src { get; set; }
            }

        }
#pragma warning restore IDE1006,CS1591 // 命名样式




        /// <summary>
        /// 百度翻译AppId
        /// </summary>
        public static string BaiduAppId { get; set; } = string.Empty;
        /// <summary>
        /// 百度翻译秘钥
        /// </summary>
        public static string BaiduKey { get; set; } = string.Empty;

        /// <summary>
        /// 百度--通用翻译api<br/>
        /// 使用前先设置<see cref="BaiduAppId"/>和<see cref="BaiduKey"/>
        /// </summary>
        /// <param name="value">需要翻译的字符</param>
        /// <param name="from">字符语种</param>
        /// <param name="to">目标语种</param>
        /// <returns>返回api的json字符，该json为<see cref="BaiduTranslateApiResult"/>，可自行序列化</returns>
        /// <exception cref="ArgumentNullException"/>
        public static string? BaiduTranslate(this string value, string from = "auto", string to = "auto")
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (BaiduAppId.IsNullOrWhiteSpace())
                return null;
            if (BaiduKey.IsNullOrWhiteSpace())
                return null;

            // 原文
            string q = value;
            // 源语言
            //string from = "en";
            // 目标语言
            //string to = "zh";
            // 改成您的APP ID
            //string appId = "";
            Random rd = new();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            //string secretKey = "";
            string sign = EncryptString(BaiduAppId + q + salt + BaiduKey);
            StringBuilder url = new("http://api.fanyi.baidu.com/api/trans/vip/translate?");
            url.Append("q=" + HttpUtility.UrlEncode(q));
            url.Append("&from=" + from);
            url.Append("&to=" + to);
            url.Append("&appid=" + BaiduAppId);
            url.Append("&salt=" + salt);
            url.Append("&sign=" + sign);

#if NET40
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.ToString());
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream myResponseStream = response.GetResponseStream();
#else
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
            var res = client.GetAsync(url.ToString()).Result;
            var myResponseStream = res.Content.ReadAsStreamAsync().Result;
#endif

            StreamReader myStreamReader = new(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 百度--语种识别api<br/>
        /// 使用前先设置<see cref="BaiduAppId"/>和<see cref="BaiduKey"/>
        /// </summary>
        /// <param name="value">要识别的字符</param>
        /// <returns></returns>
        /// <returns>返回api的json字符，该json为<see cref="BaiduLanguageApiResult"/>，可自行序列化</returns>
        /// <exception cref="ArgumentNullException"/>
        public static string? BaiduLanguage(this string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (BaiduAppId.IsNullOrWhiteSpace())
                return null;
            if (BaiduKey.IsNullOrWhiteSpace())
                return null;

            // 原文
            string q = value;
            // 源语言
            //string from = "en";
            // 目标语言
            //string to = "zh";
            // 改成您的APP ID
            //string appId = "";
            Random rd = new();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            //string secretKey = "";
            string sign = EncryptString(BaiduAppId + q + salt + BaiduKey);
            StringBuilder url = new("http://api.fanyi.baidu.com/api/trans/vip/language?");
            url.Append("q=" + HttpUtility.UrlEncode(q));
            url.Append("&appid=" + BaiduAppId);
            url.Append("&salt=" + salt);
            url.Append("&sign=" + sign);

#if NET40
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.ToString());
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream myResponseStream = response.GetResponseStream();
#else
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
            var res = client.GetAsync(url.ToString()).Result;
            var myResponseStream = res.Content.ReadAsStreamAsync().Result;
#endif

            StreamReader myStreamReader = new(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }


        // 计算MD5值
        private static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }


#if !NET40

        /// <summary>
        /// 百度--通用翻译api<br/>
        /// 使用前先设置<see cref="BaiduAppId"/>和<see cref="BaiduKey"/>
        /// </summary>
        /// <param name="value">需要翻译的字符</param>
        /// <param name="from">字符语种</param>
        /// <param name="to">目标语种</param>
        /// <returns>返回api的json字符，该json为<see cref="BaiduTranslateApiResult"/>，可自行序列化</returns>
        /// <exception cref="ArgumentNullException"/>
        public static async Task<string?> BaiduTranslateAsync(this string value, string from = "auto", string to = "auto")
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (BaiduAppId.IsNullOrWhiteSpace())
                return await Task.FromResult<string?>(null);
            if (BaiduKey.IsNullOrWhiteSpace())
                return await Task.FromResult<string?>(null);

            // 原文
            string q = value;
            // 源语言
            //string from = "en";
            // 目标语言
            //string to = "zh";
            // 改成您的APP ID
            //string appId = "";
            Random rd = new();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            //string secretKey = "";
            string sign = EncryptString(BaiduAppId + q + salt + BaiduKey);
            StringBuilder url = new("http://api.fanyi.baidu.com/api/trans/vip/translate?");
            url.Append("q=" + HttpUtility.UrlEncode(q));
            url.Append("&from=" + from);
            url.Append("&to=" + to);
            url.Append("&appid=" + BaiduAppId);
            url.Append("&salt=" + salt);
            url.Append("&sign=" + sign);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
            var res = await client.GetAsync(url.ToString());
            var myResponseStream = await res.Content.ReadAsStreamAsync();
            StreamReader myStreamReader = new(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = await myStreamReader.ReadToEndAsync();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 百度--语种识别api<br/>
        /// 使用前先设置<see cref="BaiduAppId"/>和<see cref="BaiduKey"/>
        /// </summary>
        /// <param name="value">要识别的字符</param>
        /// <returns></returns>
        /// <returns>返回api的json字符，该json为<see cref="BaiduLanguageApiResult"/>，可自行序列化</returns>
        /// <exception cref="ArgumentNullException"/>
        public static async Task<string?> BaiduLanguageAsync(this string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (BaiduAppId.IsNullOrWhiteSpace())
                return await Task.FromResult<string?>(null);
            if (BaiduKey.IsNullOrWhiteSpace())
                return await Task.FromResult<string?>(null);

            // 原文
            string q = value;
            // 源语言
            //string from = "en";
            // 目标语言
            //string to = "zh";
            // 改成您的APP ID
            //string appId = "";
            Random rd = new();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            //string secretKey = "";
            string sign = EncryptString(BaiduAppId + q + salt + BaiduKey);
            StringBuilder url = new("http://api.fanyi.baidu.com/api/trans/vip/language?");
            url.Append("q=" + HttpUtility.UrlEncode(q));
            url.Append("&appid=" + BaiduAppId);
            url.Append("&salt=" + salt);
            url.Append("&sign=" + sign);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
            var res = await client.GetAsync(url.ToString());
            var myResponseStream = await res.Content.ReadAsStreamAsync();
            StreamReader myStreamReader = new(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = await myStreamReader.ReadToEndAsync();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }



#endif
















    }


}
