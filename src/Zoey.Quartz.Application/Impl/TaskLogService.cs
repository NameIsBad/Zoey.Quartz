using Zoey.Quartz.Domain.Models;
using Zoey.Quartz.Infrastructure.Repositorys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoey.Quartz.Application
{
    public class TaskLogService : ITaskLogService
    {
        private readonly ITaskLogRepository _taskLog;

        public TaskLogService(ITaskLogRepository taskLog)
        {
            _taskLog = taskLog;
        }

        /// <summary>
        /// 获取任务日志
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskLog>> GetJobRunLog(int taskId, int page, int pageSize = 100)
        {
            return await _taskLog.GetJobRunLog(taskId, page, pageSize);
        }

        /// <summary>
        /// 记录任务日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task WriteLog(TaskLog log)
        {
            log.ExecutionTime = DateTime.Now;
            await _taskLog.WriteLog(log);
        }
    }
}
