using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zoey.Quartz.Domain.Enum;

namespace Zoey.Quartz.Domain.Model
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TaskOptions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(60)]
        public string TaskName { get; set; }
        [Required, MaxLength(60)]
        public string GroupName { get; set; }
        [Required, MaxLength(60)]
        public string Interval { get; set; }
        [Required, MaxLength(150)]
        public string ApiUrl { get; set; }
        [Required, MaxLength(250)]
        public string AuthKey { get; set; }
        [Required, MaxLength(250)]
        public string AuthValue { get; set; }
        [Required, MaxLength(250)]
        public string Describe { get; set; }
        public RequestTypeEnum RequestType { get; set; }
        public DateTime? LastRunTime { get; set; }
        public int Status { get; set; }
    }
}
