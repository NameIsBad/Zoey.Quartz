using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Zoey.Quartz.Application;
using Zoey.Quartz.Core.Quartz;
using Zoey.Quartz.Domain.Enum;
using Zoey.Quartz.Domain.Model;

namespace Zoey.Quartz
{
    public class HttpJob : ZoeyQuartzJob
    {
        private readonly ITaskService _taskService;
        private readonly ITaskLogService _taskLogService;
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Job名称
        /// </summary>
        public override string JobName => "Http请求【加Token验证】";

        public HttpJob(ITaskService taskService, ITaskLogService taskLogService, IHttpClientFactory clientFactory)
            : base()
        {
            _clientFactory = clientFactory;
            _taskLogService = taskLogService;
            _taskService = taskService;
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
                        string message = $"消息类型：{ exception.GetType().Name}\r\n消息内容：{ exception.Message}\r\n引发异常的方法：{ exception.TargetSite}\r\n引发异常源：{ exception.Source + exception.StackTrace}\r\n内部错误：{ exception.InnerException}";
                        int.TryParse(context["taskId"].ToString(), out int taskId);
                        _ = _taskLogService.WriteLog(new TaskLog(taskId, TaskLogStatusEnum.Fail, $"第{retryCount}请求失败,错误信息:{message}"));
                    });
            return p;
        }

        public async override Task Execute(IJobExecutionContext context)
        {

            AbstractTrigger trigger = context.Trigger as AbstractTrigger;
            TaskOptions taskOptions = await _taskService.GetTaskByName(trigger.Group, trigger.Name);
            if (taskOptions == null)
            {
                _ = _taskLogService.WriteLog(new TaskLog(taskOptions.Id, TaskLogStatusEnum.Fail, $"{trigger.Group}--{trigger.Name}未到找作业或可能被移除"));
                return;
            }

            if (string.IsNullOrEmpty(taskOptions.ApiUrl) || taskOptions.ApiUrl == "/")
            {
                _ = _taskLogService.WriteLog(new TaskLog(taskOptions.Id, TaskLogStatusEnum.Fail, $"{trigger.Group}--{trigger.Name}未配置url"));
            }

            await GetPolicy().ExecuteAsync(ct => ExecuteHttp(taskOptions), new Dictionary<string, object>() { { "taskId", taskOptions.Id } });
        }

        async Task ExecuteHttp(TaskOptions taskOptions)
        {
            DateTime dateTime = DateTime.Now;
            HttpResponseMessage httpMessage;
            HttpClient client = _clientFactory.CreateClient("api");

            if (!string.IsNullOrEmpty(taskOptions.AuthKey)
                && !string.IsNullOrEmpty(taskOptions.AuthValue))
            {
                client.DefaultRequestHeaders.Add(taskOptions.AuthKey.Trim(), taskOptions.AuthValue.Trim());
            }

            if (taskOptions.RequestType == RequestTypeEnum.Get)
            {
                httpMessage = await client.GetAsync(taskOptions.ApiUrl);
            }
            else
            {
                httpMessage = await client.PostAsync(taskOptions.ApiUrl, null);
            }
            TaskLog taskLog = new TaskLog()
            {
                ExecutionDuration = DateTime.Now.Subtract(dateTime).Milliseconds,
                ExecutionTime = dateTime,
                Msg = await httpMessage.Content.ReadAsStringAsync(),
                TaskId = taskOptions.Id,
                TaskLogStatus = httpMessage.IsSuccessStatusCode ? TaskLogStatusEnum.Success : TaskLogStatusEnum.Fail
            };
            _ = _taskLogService.WriteLog(taskLog);
            _ = _taskService.UpdateLastRunTime(taskOptions.Id);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("api", c =>
            {
                HttpAuthenticationHelper.SetVoucher(c);
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
    }

    #region 帮助方法
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
    #endregion
}
