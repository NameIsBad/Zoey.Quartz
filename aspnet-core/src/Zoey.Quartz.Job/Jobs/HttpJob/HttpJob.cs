using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Zoey.Quartz.Job
{
    /// <summary>
    /// HttpJob
    /// </summary>
    [ZoeyJob("HTTP请求", DataMapType = typeof(HttpJobDataMap))]
    public class HttpJob : ZoeyJob<HttpJobDataMap>
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HttpJob> _logger;
        public HttpJob() : base()
        {
        }
        public HttpJob(ILogger<HttpJob> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }


        AsyncRetryPolicy GetPolicy()
        {
            AsyncRetryPolicy p = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(new[]
                    {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                    }, (exception, timeSpan, retryCount, context) =>
                    {
                        string message = $"消息类型：{ exception.GetType().Name}\r\n消息内容：{ exception.Message}\r\n参数:{context.ToString()}\r\n引发异常的方法：{ exception.TargetSite}\r\n引发异常源：{ exception.Source + exception.StackTrace}\r\n内部错误：{ exception.InnerException}";
                        _logger.LogError(exception, message);
                    });
            return p;
        }

        public async override Task Execute(IJobExecutionContext context)
        {
            AbstractTrigger trigger = context.Trigger as AbstractTrigger;
            HttpJobDataMap data = base.GetJobData(context);
            await GetPolicy().ExecuteAsync(ct => ExecuteHttp(data), data);
        }

        async Task ExecuteHttp(HttpJobDataMap taskOptions)
        {
            DateTime dateTime = DateTime.Now;
            HttpResponseMessage httpMessage;
            HttpClient client = _clientFactory.CreateClient("api");

            if (!string.IsNullOrEmpty(taskOptions.AuthKey)
                && !string.IsNullOrEmpty(taskOptions.AuthValue))
            {
                client.DefaultRequestHeaders.Add(taskOptions.AuthKey.Trim(), taskOptions.AuthValue.Trim());
            }

            if (taskOptions.RequestType == "get")
            {
                httpMessage = await client.GetAsync(taskOptions.ApiUrl);
            }
            else
            {
                httpMessage = await client.PostAsync(taskOptions.ApiUrl, null);
            }
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("api", c =>
            {
                SetVoucher(c);
            });
            //.AddPolicyHandler(policy =>
            //{
            //    return HttpPolicyExtensions.HandleTransientHttpError()
            //                               .WaitAndRetryAsync(3,
            //                               retryAttempt => TimeSpan.FromSeconds(2),
            //                               (exception, timeSpan, retryCount, context) =>
            //                               {
            //                                   var serviceProvider = services.BuildServiceProvider();
            //                                   serviceProvider.GetService<>
            //                                   Console.ForegroundColor = ConsoleColor.Yellow;
            //                                   Console.WriteLine("请求出错了：{0} | {1} ", timeSpan, retryCount);
            //                                   Console.ForegroundColor = ConsoleColor.Gray;
            //                               });
            //});
            //.AddTransientHttpErrorPolicy(p =>
            //{
            //    p.Fallback
            //    return p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600));
            //}
            //);
        }

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