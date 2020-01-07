using Zoey.Quartz.Domain.Model;
using Zoey.Quartz.Infrastructure.Repositorys;
using System.Threading.Tasks;

namespace Zoey.Quartz.Application
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _task;

        public TaskService(ITaskRepository task)
        {
            _task = task;
        }

        /// <summary>
        /// 根据名称获取任务
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="taskName"></param>
        /// <returns></returns>
        public async Task<TaskOptions> GetTaskByName(string groupName, string taskName)
        {
            return await _task.GetTaskByName(groupName, taskName);
        }

        /// <summary>
        /// 更新最后执行时间
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task UpdateLastRunTime(int taskId)
        {
            await _task.UpdateLastRunTime(taskId);
        }
    }
}
