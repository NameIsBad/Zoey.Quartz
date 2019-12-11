using Zoey.Quartz.Application;
using Zoey.Quartz.Core.Quartz;
using Zoey.Quartz.Domain.Enum;
using Zoey.Quartz.Domain.Model;
using Zoey.Quartz.Domain.Models;
using Zoey.Quartz.Job.Utility;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zoey.Quartz
{
    public class HttpJob : ZoeyJob
    {
        private readonly ITaskService _taskService;
        private readonly ITaskLogService _taskLogService;
        private readonly IHttpClientFactory _clientFactory;

        public HttpJob() : base()
        {
        }

        public HttpJob(ITaskService taskService, ITaskLogService taskLogService, IHttpClientFactory clientFactory)
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
                ExecutionDuration = DateTime.Now.Subtract(DateTime.Now).Milliseconds,
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
}
