using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoey.Quartz.Core.PageHelper;
using Zoey.Quartz.Domain.Dto.Request;
using Zoey.Quartz.Domain.Model;
using Zoey.Quartz.Infrastructure.Repositorys;

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
        /// <returns></returns>
        public async Task<PagingQueryResult<TaskLog>> GetJobRunLog(TaskLogQueryFilter queryFilter)
        {
            return await _taskLog.GetJobRunLog(queryFilter);
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
