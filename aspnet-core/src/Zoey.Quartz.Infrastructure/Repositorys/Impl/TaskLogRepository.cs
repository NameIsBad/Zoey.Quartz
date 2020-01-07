using System.Linq;
using System.Threading.Tasks;
using Zoey.Quartz.Core.PageHelper;
using Zoey.Quartz.Domain.Dto.Request;
using Zoey.Quartz.Domain.Model;

namespace Zoey.Quartz.Infrastructure.Repositorys
{
    public class TaskLogRepository : ITaskLogRepository
    {
        /// <summary>
        /// 获取运行日志信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagingQueryResult<TaskLog>> GetJobRunLog(TaskLogQueryFilter queryFilter)
        {
            using QuartzDbContexts context = new QuartzDbContexts();
            IQueryable<TaskLog> taskLogQueryable = context.TaskLog.Where(t => t.TaskId == queryFilter.TaskId);
            return await taskLogQueryable.ToPageListAnync(queryFilter);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        public async Task WriteLog(TaskLog log)
        {
            using QuartzDbContexts context = new QuartzDbContexts();
            await context.AddAsync(log);
            await context.SaveChangesAsync();
        }
    }
}
