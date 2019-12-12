using Zoey.Quartz.Core.Quartz;
using Zoey.Quartz.Core.Web;
using Zoey.Quartz.Domain.Enum;
using Zoey.Quartz.Domain.Model;
using Zoey.Quartz.Infrastructure.Repositorys;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zoey.Quartz.Application
{
    public class QuartzService : IQuartzService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly ITaskRepository _task;
        private readonly ITaskLogRepository _taskLog;
        private readonly ILogRepository _log;
        private readonly IJobManager _jobManager;
        private readonly ITriggerManager _triggerManager;
        public QuartzService(ISchedulerFactory schedulerFactory, ITaskRepository task, IJobFactory jobFactory,
            ITaskLogRepository taskLogService, ILogRepository log,IJobManager jobManager, ITriggerManager triggerManager)
        {
            _triggerManager = triggerManager;
            _jobManager = jobManager;
            _log = log;
            _taskLog = taskLogService;
            _jobFactory = jobFactory;
            _task = task;
            _schedulerFactory = schedulerFactory;
        }

        public async Task StartQuartz()
        {
            int errorCount = 0;
            string errorMsg = "";
            TaskOptions options = null;
            var allTask = await _task.GetAll();
            try
            {
                foreach (TaskOptions item in allTask)
                {
                    options = item;
                    await AddSchedulerJob(item);
                }
            }
            catch (Exception ex)
            {
                errorCount = +1;
                errorMsg += $"作业:{options?.TaskName},异常：{ex.Message}";
            }
            string content = $"成功:{ allTask.Count() - errorCount}个,失败{errorCount}个,异常：{errorMsg}";
            _ = _log.WriteLog(content);
        }

        public async Task<IEnumerable<TaskOptions>> GetJobs()
        {
            var list = new List<TaskOptions>();
            var allTask = await _task.GetAll();

            IScheduler _scheduler = await _schedulerFactory.GetScheduler();

            IReadOnlyCollection<string> groups = await _scheduler.GetJobGroupNames();
            foreach (string groupName in groups)
            {
                foreach (JobKey jobKey in await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)))
                {
                    TaskOptions taskOptions = allTask.FirstOrDefault(t=>t.GroupName == jobKey.Group && t.TaskName == jobKey.Name);
                    if (taskOptions == null)
                    {
                        continue;
                    }
                    list.Add(taskOptions);
                }
            }
            return list;
        }

        /// <summary>
        /// 添加作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task<AjaxResponse> AddJob(TaskOptions taskOptions)
        {
            try
            {
                (bool success, string errorMsg) validExpression = IsValidExpression(taskOptions.Interval);
                if (!validExpression.success)
                {
                    return new AjaxResponse(false, validExpression.errorMsg);
                }
                var existTask = await _task.Exists(taskOptions.TaskName, taskOptions.GroupName);
                if (existTask)
                    return new AjaxResponse(false, $"作业:{taskOptions.TaskName},分组：{taskOptions.GroupName}已经存在");
                await _task.Add(taskOptions);
                //TODO:如果失败删除数据库数据
                await AddSchedulerJob(taskOptions);
                _ = _log.WriteLog($"新增任务{taskOptions.TaskName}--{taskOptions.GroupName}");
            }
            catch (Exception ex)
            {
                return new AjaxResponse(false, ex.Message);
            }
            return new AjaxResponse();
        }

        private async Task AddSchedulerJob(TaskOptions taskOptions)
        {
            IJobDetail job = _jobManager.GetJob(null)
                                  .WithIdentity(taskOptions.TaskName, taskOptions.GroupName)
                                  .Build();
            ITrigger trigger = _triggerManager.Create(null)
                               .StartNow()
                               .WithIdentity(taskOptions.TaskName, taskOptions.GroupName)
                               .WithDescription(taskOptions.Describe)
                               .WithCronSchedule(taskOptions.Interval)
                               .Build();
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;
            await scheduler.ScheduleJob(job, trigger);
            if (taskOptions.Status == (int)TriggerState.Normal)
            {
                await scheduler.Start();
            }
            else
            {
                await Pause(taskOptions);
                _ = _log.WriteLog($"作业:{taskOptions.TaskName},分组:{taskOptions.GroupName},新建时未启动原因,状态为:{taskOptions.Status}");
            }
        }

        /// <summary>
        /// 移除作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public Task<AjaxResponse> Remove(TaskOptions taskOptions)
        {
            return TriggerAction(taskOptions.TaskName, taskOptions.GroupName, JobAction.删除, taskOptions);
        }

        /// <summary>
        /// 更新作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public Task<AjaxResponse> Update(TaskOptions taskOptions)
        {
            return TriggerAction(taskOptions.TaskName, taskOptions.GroupName, JobAction.修改, taskOptions);
        }

        /// <summary>
        /// 暂停作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public Task<AjaxResponse> Pause(TaskOptions taskOptions)
        {
            return TriggerAction(taskOptions.TaskName, taskOptions.GroupName, JobAction.暂停, taskOptions);
        }

        /// <summary>
        /// 启动作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public Task<AjaxResponse> Start(TaskOptions taskOptions)
        {
            return TriggerAction(taskOptions.TaskName, taskOptions.GroupName, JobAction.开启, taskOptions);
        }

        /// <summary>
        /// 立即执行一次作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public Task<AjaxResponse> Run(TaskOptions taskOptions)
        {
            return TriggerAction(taskOptions.TaskName, taskOptions.GroupName, JobAction.立即执行, taskOptions);
        }


        /// <summary>
        /// 触发新增、删除、修改、暂停、启用、立即执行事件
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <param name="action"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        private async Task<AjaxResponse> TriggerAction(string taskName, string groupName, JobAction action, TaskOptions taskOptions = null)
        {
            string errorMsg = "";
            try
            {
                IScheduler scheduler = await _schedulerFactory.GetScheduler();
                List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)).Result.ToList();
                if (jobKeys == null || jobKeys.Count() == 0)
                {
                    errorMsg = $"未找到分组[{groupName}]";
                    return new AjaxResponse(false, errorMsg);
                }
                JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result.Any(x => (x as CronTriggerImpl).Name == taskName)).FirstOrDefault();
                if (jobKey == null)
                {
                    errorMsg = $"未找到触发器[{taskName}]";
                    return new AjaxResponse(false, errorMsg);
                }
                IReadOnlyCollection<ITrigger> triggers = await scheduler.GetTriggersOfJob(jobKey);
                ITrigger trigger = triggers?.Where(x => (x as CronTriggerImpl).Name == taskName).FirstOrDefault();

                if (trigger == null)
                {
                    errorMsg = $"未找到触发器[{taskName}]";
                    return new AjaxResponse(false, errorMsg);
                }
                AjaxResponse result = null;
                switch (action)
                {
                    case JobAction.删除:
                    case JobAction.修改:
                        await scheduler.PauseTrigger(trigger.Key);
                        await scheduler.UnscheduleJob(trigger.Key);// 移除触发器
                        await scheduler.DeleteJob(trigger.JobKey);
                        result = await ModifyTaskEntity(taskOptions, action);
                        break;
                    case JobAction.暂停:
                    case JobAction.停止:
                    case JobAction.开启:
                        result = await ModifyTaskEntity(taskOptions, action);
                        if (action == JobAction.暂停)
                        {
                            await scheduler.PauseTrigger(trigger.Key);
                        }
                        else if (action == JobAction.开启)
                        {
                            await scheduler.ResumeTrigger(trigger.Key);
                        }
                        else
                        {
                            await scheduler.Shutdown();
                        }
                        break;
                    case JobAction.立即执行:
                        await scheduler.TriggerJob(jobKey);
                        break;
                }
                return new AjaxResponse(true, $"作业{action.ToString()}成功");
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return new AjaxResponse(false, ex.Message);
            }
        }

        private async Task<AjaxResponse> ModifyTaskEntity(TaskOptions taskOptions, JobAction action)
        {
            var result = new AjaxResponse();
            switch (action)
            {
                case JobAction.删除:
                    await _task.Delete(taskOptions.Id);
                    break;
                case JobAction.修改:
                    await _task.Edit(taskOptions);
                    //生成任务并添加新配置
                    await AddSchedulerJob(taskOptions);
                    break;
                case JobAction.暂停:
                case JobAction.开启:
                case JobAction.停止:
                case JobAction.立即执行:
                    TaskOptions options = await _task.GetTaskById(taskOptions.Id);
                    if (action == JobAction.暂停)
                    {
                        options.Status = (int)TriggerState.Paused;
                    }
                    else if (action == JobAction.停止)
                    {
                        options.Status = (int)action;
                    }
                    else
                    {
                        options.Status = (int)TriggerState.Normal;
                    }

                    await _task.Edit(options);
                    break;
            }
            //生成配置文件
            _ = _log.WriteLog($"[{action}]任务：{taskOptions.GroupName}--[taskOptions.TaskName]，操作对象：{JsonConvert.SerializeObject(taskOptions)}");
            return result;
        }


        private (bool success, string errorMsg) IsValidExpression(string cronExpression)
        {
            try
            {
                CronTriggerImpl trigger = new CronTriggerImpl
                {
                    CronExpressionString = cronExpression
                };
                DateTimeOffset? date = trigger.ComputeFirstFireTimeUtc(null);
                return (date != null, date == null ? $"请确认表达式{cronExpression}是否正确!" : "");
            }
            catch (Exception e)
            {
                return (false, $"请确认表达式{cronExpression}是否正确!{e.Message}");
            }
        }
    }
}
