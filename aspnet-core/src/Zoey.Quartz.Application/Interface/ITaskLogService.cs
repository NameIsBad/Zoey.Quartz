using System.Threading.Tasks;
using Zoey.Quartz.Core.Application.Services;
using Zoey.Quartz.Core.PageHelper;
using Zoey.Quartz.Domain.Dto.Request;
using Zoey.Quartz.Domain.Model;

namespace Zoey.Quartz.Application
{
    public interface ITaskLogService : IApplicationService
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
