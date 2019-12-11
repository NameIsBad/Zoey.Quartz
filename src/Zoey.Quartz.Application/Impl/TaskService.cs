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

        public async Task<TaskOptions> GetTaskByName(string groupName, string taskName)
        {
            return await _task.GetTaskByName(groupName, taskName);
        }

        public async Task UpdateLastRunTime(int taskId)
        {
            await _task.UpdateLastRunTime(taskId);
        }
    }
}
