using System;
using System.Threading.Tasks;
using Zoey.Quartz.Domain.Model;

namespace Zoey.Quartz.Infrastructure.Repositorys
{
    public class LogRepository : ILogRepository
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        public async Task WriteLog(Log log)
        {
            using var context = new QuartzDbContexts();
            await context.AddAsync(log);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task WriteLog(string content)
        {
            Log log = new Log
            {
                Content = content,
                ClientIpAddress = "",
                ExecutionTime = DateTime.Now,
                BrowserInfo = "",
                UserName = ""
            };
            await WriteLog(log);
        }
    }
}
