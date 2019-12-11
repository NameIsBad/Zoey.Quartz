using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Zoey.Quartz.Job.Utility
{
    public class HttpAuthenticationHelper
    {
        #region 权限
        private const string FrameworkCallKey = "80947DEE-F900-460F-8BC7-D63BC9CC6BAB";
        private static string GetTimestamp(DateTime date)
        {
            TimeSpan ts = date - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        private static string GetToken(string timestamp)
        {
            return Md5Utils.MD5Encoding(FrameworkCallKey, timestamp);
        }
        /// <summary>
        /// 设置请求凭证
        /// </summary>
        /// <param name="client"></param>
        public static void SetVoucher(HttpClient client)
        {
            string timestamp = GetTimestamp(DateTime.UtcNow);
            string token = GetToken(timestamp);
            client.DefaultRequestHeaders.TryAddWithoutValidation("token", token);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
        }
        #endregion
    }

    public class Md5Utils
    {

        /// <summary>
        /// MD5 加密字符串
        /// </summary>
        /// <param name="rawPass">源字符串</param>
        /// <returns>加密后字符串</returns>
        public static string MD5Encoding(string rawPass)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5盐值加密
        /// </summary>
        /// <param name="rawPass">源字符串</param>
        /// <param name="salt">盐值</param>
        /// <returns>加密后字符串</returns>
        public static string MD5Encoding(string rawPass, object salt)
        {
            if (salt == null)
            {
                return rawPass;
            }

            return MD5Encoding(rawPass + "{" + salt.ToString() + "}");
        }
    }
}
