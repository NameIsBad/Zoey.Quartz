using Dapper;
using Zoey.Quartz.Domain.Model;
using System;
using System.Data.SQLite;
using System.Threading.Tasks;

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
            string sql = @"INSERT INTO Log (ExecutionTime,ClientIpAddress,BrowserInfo,UserName,Content) VALUES
                        (@ExecutionTime,@ClientIpAddress,@BrowserInfo,@UserName,@Content)";
            using var con = new DapperDbContexts().Connection;
            await con.ExecuteAsync(sql, log);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task WriteLog(string content)
        {
            var log = new Log
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
