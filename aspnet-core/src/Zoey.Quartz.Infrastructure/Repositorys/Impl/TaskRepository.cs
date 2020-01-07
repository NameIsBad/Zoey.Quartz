using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Zoey.Quartz.Core.PageHelper;
using Zoey.Quartz.Domain.Dto.Request;
using Zoey.Quartz.Domain.Model;

namespace Zoey.Quartz.Infrastructure.Repositorys
{
    public class TaskRepository : ITaskRepository
    {
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TaskOptions>> GetAll()
        {
            using var context = new QuartzDbContexts();
            return await context.TaskOptions.ToListAsync();
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        public async Task<PagingQueryResult<TaskOptions>> GetPage(TaskOptionsQueryFilter queryFilter)
        {
            using var context = new QuartzDbContexts();
            var queryable = context.TaskOptions.AsQueryable();
            queryable = queryable.WhereIf(!string.IsNullOrWhiteSpace(queryFilter.TaskName), t => t.TaskName.Contains(queryFilter.TaskName));
            return await queryable.ToPageListAnync(queryFilter);
        }

        /// <summary>
        /// 根据<see cref="JobKey"/>获取任务
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<TaskOptions> GetTaskByJobKey(JobKey jobKey)
        {
            return await GetTaskByName(jobKey.Group, jobKey.Name);
        }

        /// <summary>
        /// 根据<see cref="TaskOptions"/>获取任务
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<TaskOptions> GetTaskByOptions(TaskOptions option)
        {
            return await GetTaskByName(option.GroupName, option.TaskName);
        }

        /// <summary>
        /// 根据名称获取任务
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="taskName"></param>
        /// <returns></returns>
        public async Task<TaskOptions> GetTaskByName(string groupName, string taskName)
        {
            using var context = new QuartzDbContexts();
            return await context.TaskOptions.FirstOrDefaultAsync(t => t.TaskName == taskName && t.GroupName == groupName);
        }
        /// <summary>
        /// 根据ID获取任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskOptions> GetTaskById(int id)
        {
            using var context = new QuartzDbContexts();
            return await context.TaskOptions.FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task Add(TaskOptions task)
        {
            using var context = new QuartzDbContexts();
            await context.TaskOptions.AddAsync(task);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            using var context = new QuartzDbContexts();
            await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {nameof(TaskOptions)} WHERE {nameof(TaskOptions.Id)} = {id}");
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task Edit(TaskOptions task)
        {
            using var context = new QuartzDbContexts();
            await context.Database.ExecuteSqlInterpolatedAsync(@$"UPDATE TaskOptions SET 
                                                                TaskName = {task.TaskName},
                                                                GroupName = {task.GroupName},
                                                                Interval = {task.Interval},
                                                                ApiUrl = {task.ApiUrl},
                                                                AuthKey = {task.AuthKey},
                                                                AuthValue = {task.AuthValue},
                                                                Describe = {task.Describe},
                                                                RequestType = {task.RequestType},
                                                                LastRunTime = {task.LastRunTime},
                                                                Status = {task.Status} 
                                                                WHERE Id = {task.Id}");
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 根据任务名称和分组名称判断是否存在任务
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task<bool> Exists(string taskName, string groupName)
        {
            using var context = new QuartzDbContexts();
            return await context.TaskOptions.AnyAsync(t => t.TaskName == taskName && t.GroupName == groupName);
        }

        /// <summary>
        /// 更新最后执行时间
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task UpdateLastRunTime(int taskId)
        {
            using var context = new QuartzDbContexts();
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE TaskOptions SET LastRunTime = {DateTime.Now} WHERE Id = {taskId}");
            await context.SaveChangesAsync();
        }
    }
}
