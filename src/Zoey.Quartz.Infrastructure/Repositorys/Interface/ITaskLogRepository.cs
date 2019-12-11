using Zoey.Quartz.Core.Domain.Repositories;
using Zoey.Quartz.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoey.Quartz.Infrastructure.Repositorys
{
    public interface ITaskLogRepository : IRepository
    {
        /// <summary>
        /// 获取运行日志信息
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<TaskLog>> GetJobRunLog(int taskId, int page, int pageSize = 100);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        Task WriteLog(TaskLog log);
    }
}
