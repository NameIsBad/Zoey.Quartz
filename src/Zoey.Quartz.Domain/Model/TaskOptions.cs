using Zoey.Quartz.Domain.Enum;
using System;

namespace Zoey.Quartz.Domain.Model
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TaskOptions
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string GroupName { get; set; }
        public string Interval { get; set; }
        public string ApiUrl { get; set; }
        public string AuthKey { get; set; }
        public string AuthValue { get; set; }
        public string Describe { get; set; }
        public RequestTypeEnum RequestType { get; set; }
        public DateTime? LastRunTime { get; set; }
        public int Status { get; set; }
    }
}
