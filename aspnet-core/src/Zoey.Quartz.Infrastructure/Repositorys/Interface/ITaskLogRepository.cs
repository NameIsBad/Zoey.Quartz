using System.Threading.Tasks;
using Zoey.Quartz.Core.Domain.Repositories;
using Zoey.Quartz.Core.PageHelper;
using Zoey.Quartz.Domain.Dto.Request;
using Zoey.Quartz.Domain.Model;

namespace Zoey.Quartz.Infrastructure.Repositorys
{
    public interface ITaskLogRepository : IRepository
    {
        /// <summary>
        /// 获取运行日志信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagingQueryResult<TaskLog>> GetJobRunLog(TaskLogQueryFilter queryFilter);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        Task WriteLog(TaskLog log);
    }
}
