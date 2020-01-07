using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zoey.Quartz.Domain.Enum;

namespace Zoey.Quartz.Domain.Model
{
    /// <summary>
    /// 日志
    /// </summary>
    public class TaskLog
    {
        public TaskLog()
        {

        }

        public TaskLog(int taskId, TaskLogStatusEnum taskLogStatus, string msg)
            : this(taskId, taskLogStatus, DateTime.Now, 0, msg)
        {
        }

        public TaskLog(int taskId, TaskLogStatusEnum taskLogStatus, int executionDuration, string msg)
            : this(taskId, taskLogStatus, DateTime.Now, executionDuration, msg)
        {
        }
        public TaskLog(int taskId, TaskLogStatusEnum taskLogStatus, DateTime executionTime, int executionDuration, string msg)
        {
            TaskId = taskId;
            ExecutionTime = executionTime;
            ExecutionDuration = executionDuration;
            TaskLogStatus = taskLogStatus;
            Msg = msg;
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int TaskId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }
        /// <summary>
        /// 执行时间(毫秒)
        /// </summary>
        public int ExecutionDuration { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public TaskLogStatusEnum TaskLogStatus { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        [MaxLength(1000)]
        public string Msg { get; set; }
    }
}
