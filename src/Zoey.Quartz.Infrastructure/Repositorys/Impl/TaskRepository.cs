using Dapper;
using Zoey.Quartz.Domain.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

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
            string sql = @"SELECT * FROM TaskOptions";
            using var con = new DapperDbContexts().Connection;
            IEnumerable<TaskOptions> tasks = await con.QueryAsync<TaskOptions>(sql);
            return tasks;
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
            string sql = @"SELECT * FROM TaskOptions WHERE TaskName = @taskName AND GroupName = @groupName";
            using var con = new DapperDbContexts().Connection;
            TaskOptions task = await con.QueryFirstOrDefaultAsync<TaskOptions>(sql, new
            {
                taskName = taskName,
                groupName = groupName
            });
            return task;
        }
        /// <summary>
        /// 根据ID获取任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskOptions> GetTaskById(int id)
        {
            string sql = @"SELECT * FROM TaskOptions WHERE Id = @id";
            using var con = new DapperDbContexts().Connection;
            TaskOptions task = await con.QueryFirstOrDefaultAsync<TaskOptions>(sql, new
            {
                id = id
            });
            return task;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task Add(TaskOptions task)
        {
            string sql = @"INSERT INTO TaskOptions(TaskName,GroupName,Interval,ApiUrl,AuthKey,AuthValue,Describe,RequestType,LastRunTime,Status)
                        VALUES (@TaskName,@GroupName,@Interval,@ApiUrl,@AuthKey,@AuthValue,@Describe,@RequestType,@LastRunTime,@Status)";
            using var con = new DapperDbContexts().Connection;
            await con.ExecuteAsync(sql, task);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            string sql = @"DELETE FROM TaskOptions WHERE Id = @id";
            using var con = new DapperDbContexts().Connection;
            await con.ExecuteAsync(sql, new { id = id });
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task Edit(TaskOptions task)
        {
            string sql = @"UPDATE TaskOptions SET TaskName = @TaskName,GroupName = @GroupName,Interval = @Interval
                    ,ApiUrl = @ApiUrl,AuthKey = @AuthKey,AuthValue = @AuthValue,Describe = @Describe,RequestType = @RequestType
                    ,LastRunTime = @LastRunTime,Status = @Status WHERE Id = @Id";
            using var con = new DapperDbContexts().Connection;
            await con.ExecuteAsync(sql, task);
        }

        /// <summary>
        /// 根据任务名称和分组名称判断是否存在任务
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task<bool> Exists(string taskName, string groupName)
        {
            var sql = @"SELECT COUNT(*) FROM TaskOptions WHERE TaskName = @taskName AND GroupName = @groupName";
            using var con = new DapperDbContexts().Connection;
            var count = await con.ExecuteScalarAsync<int>(sql, new
            {
                taskName = taskName,
                groupName = groupName
            });
            return count > 0;
        }

        /// <summary>
        /// 更新最后执行时间
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task UpdateLastRunTime(int taskId)
        {
            string sql = @"UPDATE TaskOptions SET LastRunTime = @LastRunTime WHERE Id = @id";
            using var con = new DapperDbContexts().Connection;
            await con.ExecuteAsync(sql, new { LastRunTime = DateTime.Now, id = taskId });
        }
    }
}
