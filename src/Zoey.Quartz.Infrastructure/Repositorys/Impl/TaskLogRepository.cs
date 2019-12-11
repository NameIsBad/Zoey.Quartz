using Dapper;
using Zoey.Quartz.Domain.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Zoey.Quartz.Infrastructure.Repositorys
{
    public class TaskLogRepository : ITaskLogRepository
    {
        /// <summary>
        /// 获取运行日志信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskLog>> GetJobRunLog(int taskId, int page, int pageSize = 30)
        {
            string sql = $@"SELECT * FROM TaskLog WHERE TaskId = @taskId ORDER BY ExecutionTime DESC LIMIT {pageSize} OFFSET {(page - 1) * pageSize}";
            using var con = new DapperDbContexts().Connection;
            IEnumerable<TaskLog> result = await con.QueryAsync<TaskLog>(sql, new { taskId = taskId });
            return result;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        public async Task WriteLog(TaskLog log)
        {
            string sql = @"INSERT INTO TaskLog (TaskId,ExecutionTime,ExecutionDuration,Msg,TaskLogStatus) VALUES
                        (@TaskId,@ExecutionTime,@ExecutionDuration,@Msg,@TaskLogStatus)";
            using var con = new DapperDbContexts().Connection;
            await con.ExecuteAsync(sql, log);
        }
    }
}
