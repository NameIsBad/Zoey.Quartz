using Zoey.Quartz.Core.Application.Services;
using Zoey.Quartz.Domain.Model;
using System.Threading.Tasks;

namespace Zoey.Quartz.Application
{
    public interface ITaskService : IApplicationService
    {

        /// <summary>
        /// 根据名称获取任务
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="taskName"></param>
        /// <returns></returns>
        Task<TaskOptions> GetTaskByName(string groupName, string taskName);
        /// <summary>
        /// 更新最后执行时间
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task UpdateLastRunTime(int taskId);
    }
}
