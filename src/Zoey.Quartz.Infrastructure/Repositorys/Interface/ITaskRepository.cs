using Zoey.Quartz.Core.Domain.Repositories;
using Zoey.Quartz.Domain.Model;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoey.Quartz.Infrastructure.Repositorys
{
    public interface ITaskRepository : IRepository
    {
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TaskOptions>> GetAll();
        /// <summary>
        /// 根据任务名称和分组名称判断是否存在任务
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<bool> Exists(string taskName, string groupName);
        /// <summary>
        /// 根据<see cref="JobKey"/>获取任务
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        Task<TaskOptions> GetTaskByJobKey(JobKey jobKey);
        /// <summary>
        /// 根据<see cref="TaskOptions"/>获取任务
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<TaskOptions> GetTaskByOptions(TaskOptions option);
        /// <summary>
        /// 根据名称获取任务
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="taskName"></param>
        /// <returns></returns>
        Task<TaskOptions> GetTaskByName(string groupName, string taskName);
        /// <summary>
        /// 根据ID获取任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskOptions> GetTaskById(int id);
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task Add(TaskOptions task);
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task Delete(int id);
        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task Edit(TaskOptions task);
        /// <summary>
        /// 更新最后执行时间
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task UpdateLastRunTime(int taskId);

    }
}
